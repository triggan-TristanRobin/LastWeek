using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Tools;
using System.Text.RegularExpressions;
using LastWeek.Model;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Validation;
using System.Threading.Tasks;
using ReviewExporter.Interfaces;
using Microsoft.Extensions.Logging;

namespace ReviewExporter
{
    public class MainExporter : IReviewExporter
    {
        public static string ApplicationDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string ResourcesPath => Path.Combine(ApplicationDataPath, @"WeeklyReviewer\Resources\");
        public static string ReviewFileName = "WeeklyReview.docx";
        public static string ReviewFilePath => ResourcesPath + ReviewFileName;
        public IFileSaver FileSaver { get; set; }

        private string name;
        private string mimeType;
        private string description;
        private byte[] reviewFile;
        private Review review;

        private readonly ILogger<MainExporter> logger;

        public MainExporter(ILogger<MainExporter> logger, IFileSaver fileSaver)
        {
            this.logger = logger;
            FileSaver = fileSaver;
            CreateFileIfNeeded();
        }

        public void CreateFileIfNeeded()
        {
            logger.LogInformation("Checking needed files presence");
            if (!Directory.Exists(ResourcesPath))
                Directory.CreateDirectory(ResourcesPath);
            if (!File.Exists(Path.Combine(ResourcesPath, "Labels.list")))
                using (File.Create(Path.Combine(ResourcesPath, "Labels.list"))) { }
        }

        public void WriteDoc(Review review)
        {
            this.review = review;
            logger.LogInformation("Records saving (using docx file)");
            // Create Stream
            using (MemoryStream mem = new MemoryStream())
            {
                // Create Document
                using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    // Add a main document part. 
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    // Create the document structure and add some text.
                    mainPart.Document = new Document();
                    Body docBody = new Body();

                    // Add your docx content here
                    Paragraph p = new Paragraph();
                    Run r = new Run();
                    r.Append(new RunProperties
                    {
                        Bold = new Bold(),
                        FontSize = new FontSize { Val = new StringValue("48") }
                    });
                    Text t = new Text($"From {review.StartDate.ToLongDateString()} to {review.EndDate.ToLongDateString()}");
                    r.Append(t);
                    p.Append(r);
                    docBody.Append(p);
                    mainPart.Document.Append(docBody);
                    SetRecords(review.Records, docBody);

                    OpenXmlValidator validator = new OpenXmlValidator();
                    int count = 0;
                    foreach (ValidationErrorInfo error in
                        validator.Validate(wordDocument))
                    {
                        count++;
                        logger.LogError("-------------------------------------------");
                        logger.LogError("Validation error");
                        logger.LogError("Error " + count);
                        logger.LogError("Description: " + error.Description);
                        logger.LogError("ErrorType: " + error.ErrorType);
                        logger.LogError("Node: " + error.Node);
                        logger.LogError("Path: " + error.Path.XPath);
                        logger.LogError("Part: " + error.Part.Uri);
                        logger.LogError("-------------------------------------------");
                    }



                    wordDocument.Save();
                }

                if (File.Exists(ReviewFilePath))
                    File.Delete(ReviewFilePath);
                using (var file = new FileStream(ReviewFilePath, FileMode.OpenOrCreate))
                {
                    logger.LogInformation($"Saving stream into file at {ReviewFilePath}");
                    mem.Position = 0;
                    mem.CopyTo(file);
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Print the given records on the doc and save it
        /// </summary>
        /// <param name="records"></param>
        public void SetRecords(List<Record> records, Body docBody)
        {
            logger.LogInformation("Adding records to generated doc");
            foreach (var record in records)
            {
                var question = record.Question;
                logger.LogInformation($"Question:\n----{question}\n----");
                // Add your docx content here
                Paragraph p = new Paragraph();
                p.Append(new ParagraphProperties
                {
                    SpacingBetweenLines = new SpacingBetweenLines { Line = new StringValue("240"), LineRule = new EnumValue<LineSpacingRuleValues>(LineSpacingRuleValues.Auto) },
                });
                Run r = new Run();
                r.Append(new RunProperties
                {
                    Bold = new Bold(),
                });
                Text t = new Text(question);
                r.Append(t);
                p.Append(r);
                docBody.Append(p);
                var list = record.AnswersAsTextEnumerable();
                foreach (var formatedAnswer in list)
                {
                    Paragraph p2 = new Paragraph();
                    p2.Append(new ParagraphProperties
                    {
                        ParagraphStyleId = new ParagraphStyleId { Val = new StringValue("Paragraphedeliste") },
                        NumberingProperties = new NumberingProperties
                        {
                            NumberingLevelReference = new NumberingLevelReference { Val = new Int32Value(0) },
                            NumberingId = new NumberingId { Val = new Int32Value(3) }
                        }
                    });
                    Run r2 = new Run();
                    r2.Append(new RunProperties
                    {
                        FontSize = new FontSize { Val = new StringValue("20") },
                        Italic = new Italic()
                    });
                    Text t2 = new Text(formatedAnswer);
                    r2.Append(t2);
                    p2.Append(r2);
                    docBody.Append(p2);
                }
            }
        }

        //public IEnumerable<Result> StoreReview(List<string> paths, List<bool> saversEnabled, List<List<object>> additionalParams)
        //{
        //    log.LogInformation("Store the current review with given saving params");
        //    if (GetFile())
        //    {
        //        log.LogInformation("Retrieved the review file");
        //        for (int i = 0; i < FileSaver.Count; i++)
        //        {
        //            var enabledSaving = i >= saversEnabled.Count || saversEnabled[i];
        //            log.LogInformation($"Will {(enabledSaving ? "" : "not ")}save using {FileSaver[i].GetType().Name}");
        //            if (enabledSaving)
        //            {
        //                var fs = FileSaver[i];
        //                yield return fs.SaveFile(reviewFile, name, mimeType, description,
        //                    i >= paths.Count ? string.Empty : paths[i],
        //                    additionalParams == null ? null : i >= additionalParams.Count ? null : additionalParams[i]);
        //            }
        //        }
        //    }
        //}

        public bool GetFile()
        {
            name = mimeType = description = "";
            var date = review.StartDate.ToString("yyyy-MM-dd");
            if (File.Exists(ReviewFilePath))
            {
                name = review.GetPeriod().ToString().ToUpper()[0].ToString() + date + Path.GetExtension(ReviewFilePath);
                description = review.GetPeriod() + " review from " + date;
                reviewFile = File.ReadAllBytes(ReviewFilePath);
                return reviewFile != null;
            }
            return false;
        }

        public void SaveConfig(List<string> list)
        {
            logger.LogInformation("Label list saving");
            using (var labelFile = new StreamWriter(Path.Combine(ResourcesPath, "Labels.list")))
            {
                list.ForEach(question => labelFile.WriteLine(question));
            }
        }

        //public async Task<List<Result>> StoreReviewAsync(List<string> paths, List<bool> saversEnabled, List<List<object>> additionalParams)
        //{
        //    var returnList = new List<Result>();
        //    log.LogInformation("Async store the current review with given saving params");
        //    if (GetFile())
        //    {
        //        log.LogInformation("Retrieved the review file");
        //        for (int i = 0; i < FileSaver.Count; i++)
        //        {
        //            var enabledSaving = i >= saversEnabled.Count || saversEnabled[i];
        //            log.LogInformation($"Will {(enabledSaving ? "" : "not ")}save using {FileSaver[i].GetType().Name}");
        //            if (enabledSaving)
        //            {
        //                var fs = FileSaver[i];
        //                var result = await fs.SaveFileAsync(reviewFile, name, mimeType, description,
        //                    i >= paths.Count ? string.Empty : paths[i],
        //                    additionalParams == null ? null : i >= additionalParams.Count ? null : additionalParams[i]);
        //                returnList.Add(result);
        //            }
        //        }
        //    }
        //    return returnList;
        //}

        public async Task<Result> StoreReviewAsync(string path, List<object> additionalParams)
        {
            Result result = null;
            logger.LogInformation("Async store the current review with specific filesaver (by index)");
            if (GetFile())
            {
                logger.LogInformation("Retrieved the review file");
                logger.LogInformation($"Will save using {FileSaver.GetType().Name}");
                result = await FileSaver.SaveFileAsync(reviewFile, name, mimeType, description, path, additionalParams.ToArray());
            }
            return result;
        }
    }
}
