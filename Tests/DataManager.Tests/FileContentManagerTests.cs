using LastWeek.Model;
using Moq;
using DataManager;
using NUnit.Framework;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using DataManager.Tests;
using System.Threading.Tasks;
using LastWeek.Model.Enums;

namespace DataManagerTests
{
    [TestFixture]
    public class FileContentManagerTests
    {
        public IAsyncContentManager ContentManager { get; set; }

        [Test]
        public async Task GetReviewsShouldReturnEmptyListIfFileNotExists()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            string path = Path.Combine(Environment.CurrentDirectory, "data", "donotexist.json");
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);

            // Act
            List<Review> reviews = await contentManager.GetReviewsAsync();

            // Assert
            Assert.IsFalse(reviews.Any());
        }

        [Test]
        public async Task GetReviewsShouldReturnEmptyListIfFileIsEmpty()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(FileStrings.GetFile("empty"));
            string path = @"C:\tmp\empty.json";
            mockFileSystem.AddFile(path, mockInputFile);
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);

            // Act
            List<Review> reviews = await contentManager.GetReviewsAsync();

            // Assert
            Assert.IsFalse(reviews.Any());
        }

        [Test]
        [TestCase("oneReview", 1)]
        [TestCase("twoReviews", 2)]
        [TestCase("tenReviews", 10)]
        public async Task GetReviewsShouldReturnStoredListIfFileIsCorrect(string fileName, int expectedCount)
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(FileStrings.GetFile(fileName));
            string path = $@"C:\tmp\{fileName}.json";
            mockFileSystem.AddFile(path, mockInputFile);
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);

            // Act
            List<Review> reviews = await contentManager.GetReviewsAsync();

            // Assert
            Assert.AreEqual(expectedCount, reviews.Count());
        }

        [Test]
        public async Task GetReviewShouldReturnNullIfFileNotExists()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            string path = Path.Combine(Environment.CurrentDirectory, "data", "donotexist.json");
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);

            // Act
            Review review = await contentManager.GetReviewAsync(It.IsAny<Guid>());

            // Assert
            Assert.IsNull(review);
        }

        [Test]
        public async Task GetReviewShouldReturnNullIfFileIsEmpty()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(FileStrings.GetFile("empty"));
            string path = @"C:\tmp\empty.json";
            mockFileSystem.AddFile(path, mockInputFile);
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);

            // Act
            Review review = await contentManager.GetReviewAsync(It.IsAny<Guid>());

            // Assert
            Assert.IsNull(review);
        }

        [Test]
        [TestCase("oneReview", "30394e3f-e9bc-44ba-a889-0c9d781f93ff", ReviewStatus.New)]
        [TestCase("twoReviews", "f92c9883-a2cf-4adb-bb69-f34b6ab29aad", ReviewStatus.Active)]
        [TestCase("tenReviews", "1e2c617c-5e23-41a4-bc32-c2d8e73c683f", ReviewStatus.Validated)]
        [TestCase("tenReviews", "64941463-e6d1-4053-8619-9ba6b2882154", ReviewStatus.Active)]
        public async Task GetReviewShouldReturnReviewIfOnlyMatch(string fileName, string guidStr, ReviewStatus expectedStatus)
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(FileStrings.GetFile(fileName));
            string path = $@"C:\tmp\{fileName}.json";
            mockFileSystem.AddFile(path, mockInputFile);
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);
            Guid guid = new Guid(guidStr);

            // Act
            Review review = await contentManager.GetReviewAsync(guid);

            // Assert
            Assert.AreEqual(guid, review.Guid);
            Assert.AreEqual(expectedStatus, review.Status);
            Assert.AreEqual(new DateTime(2009, 02, 15), review.StartDate);
            Assert.AreEqual(new DateTime(2009, 02, 15), review.EndDate);
            Assert.IsNotNull(review.Records);
            Assert.IsFalse(review.Records.Any());
        }

        [Test]
        public void GetReviewShouldtThrowExceptionIfMultipleMatches()
        {
            // Arrange
            string fileName = "doubleReviews";
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(FileStrings.GetFile(fileName));
            string path = $@"C:\tmp\{fileName}.json";
            mockFileSystem.AddFile(path, mockInputFile);
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);
            Guid guid = new Guid("7176da9a-3670-4fe3-8d11-cb19d697620e");

            // Act
            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await contentManager.GetReviewAsync(guid));
        }

        [Test]
        public void GetReviewShouldThrowExceptionWhenNoMatch()
        {
            // Arrange
            string fileName = "oneReview";
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(FileStrings.GetFile(fileName));
            string path = $@"C:\tmp\{fileName}.json";
            mockFileSystem.AddFile(path, mockInputFile);
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);
            Guid guid = new Guid("7176da9a-3670-4fe3-8d11-cb19d697620e");

            // Act
            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await contentManager.GetReviewAsync(guid));
        }

        [Test]
        public async Task UpsertReviewShouldCreateJsonfileWithReviewIfNoneExist()
        {
            // Arrange
            string fileName = "savedReview";
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddDirectory(@"C:\tmp\");
            string path = $@"C:\tmp\{fileName}.json";
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);
            Guid guid = new Guid("7176da9a-3670-4fe3-8d11-cb19d697620e");
            Review reviewToSave = new()
            {
                Guid = guid,
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };

            // Act
            await contentManager.UpsertReviewAsync(reviewToSave);
            Review savedReview = (await contentManager.GetReviewsAsync())[0];

            // Assert
            Assert.IsTrue(mockFileSystem.File.Exists(path));
            Assert.AreEqual(reviewToSave, savedReview);
        }

        [Test]
        public async Task UpsertReviewShouldAddReviewToFileIfExistsWithoutSameReview()
        {
            // Arrange
            string fileName = "oneReview";
            var mockFileSystem = new MockFileSystem();
            string path = $@"C:\tmp\{fileName}.json";
            var mockInputFile = new MockFileData(FileStrings.GetFile(fileName));
            mockFileSystem.AddFile(path, mockInputFile);
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);
            Guid guid = new("7176da9a-3670-4fe3-8d11-cb19d697620e");
            Review reviewToSave = new()
            {
                Guid = guid,
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };

            // Act
            await contentManager.UpsertReviewAsync(reviewToSave);
            List<Review> reviews = await contentManager.GetReviewsAsync();
            Review savedReview = reviews.FirstOrDefault(r => r.Guid == guid);

            // Assert
            Assert.IsTrue(mockFileSystem.File.Exists(path));
            Assert.AreEqual(2, reviews.Count);
            Assert.AreEqual(reviewToSave, savedReview);
        }

        //[Test]
        //public void PostReviewShouldNotAddReviewToFileIfExistsWithSameReview()
        //{
        //    // Arrange
        //    string fileName = "twoReviews";
        //    var mockFileSystem = new MockFileSystem();
        //    string path = $@"C:\tmp\{fileName}.json";
        //    var mockInputFile = new MockFileData(FileStrings.GetFile(fileName));
        //    mockFileSystem.AddFile(path, mockInputFile);
        //    IContentManager contentManager = new ContentManager(path, mockFileSystem);
        //    Guid guid = new("7176da9a-3670-4fe3-8d11-cb19d697620e");
        //    Review reviewToSave = new()
        //    {
        //        Guid = guid,
        //        EndDate = DateTime.MaxValue,
        //        StartDate = DateTime.Today,
        //        Status = ReviewStatus.Active,
        //        Entries = new List<Entry>()
        //    };

        //    // Act
        //    bool result = contentManager.PostReview(reviewToSave);
        //    List<Review> reviews = contentManager.GetReviews();
        //    Review savedReview = reviews.FirstOrDefault(r => r.Guid == guid);

        //    // Assert
        //    Assert.IsFalse(result);
        //    Assert.IsTrue(mockFileSystem.File.Exists(path));
        //    Assert.AreEqual(2, reviews.Count);
        //    Assert.AreEqual(DateTime.Parse("2009-02-15"), savedReview.EndDate);
        //}

        //[Test]
        //public void PutReviewShouldThrowExceptionIfNoFileOrEmpty()
        //{
        //    // Arrange
        //    string fileName = "donotexist";
        //    var mockFileSystem = new MockFileSystem();
        //    mockFileSystem.AddDirectory(@"C:\tmp\");
        //    string path = $@"C:\tmp\{fileName}.json";
        //    IContentManager contentManager = new ContentManager(path, mockFileSystem);
        //    Guid guid = new Guid("7176da9a-3670-4fe3-8d11-cb19d697620e");
        //    Review reviewToSave = new()
        //    {
        //        Guid = guid,
        //        EndDate = DateTime.MaxValue,
        //        StartDate = DateTime.Today,
        //        Status = ReviewStatus.Active,
        //        Entries = new List<Entry>()
        //    };

        //    // Act
        //    // Assert
        //    Assert.Throws<FileNotFoundException>(() => contentManager.PutReview(reviewToSave));
        //}

        //[Test]
        //[TestCase("empty", "30394e3f-e9bc-44ba-a889-0c9d781f93ff", 0)]
        //[TestCase("oneReview", "7176da9a-3670-4fe3-8d11-cb19d697620e", 1)]
        //[TestCase("tenReviews", "30394e3f-e9bc-44ba-a889-0c9d781f93ff", 10)]
        //public void PutReviewShouldNotChangeFileIfFileWithoutSameReview(string fileName, string guidStr, int itemCount)
        //{
        //    // Arrange
        //    var mockFileSystem = new MockFileSystem();
        //    mockFileSystem.AddDirectory(@"C:\tmp\");
        //    string path = $@"C:\tmp\{fileName}.json";
        //    var mockInputFile = new MockFileData(FileStrings.GetFile(fileName));
        //    mockFileSystem.AddFile(path, mockInputFile);
        //    IContentManager contentManager = new ContentManager(path, mockFileSystem);
        //    Guid guid = new Guid(guidStr);
        //    Review reviewToSave = new()
        //    {
        //        Guid = guid,
        //        EndDate = DateTime.MaxValue,
        //        StartDate = DateTime.Today,
        //        Status = ReviewStatus.Active,
        //        Entries = new List<Entry>()
        //    };

        //    // Act
        //    var result = contentManager.PutReview(reviewToSave);
        //    List<Review> reviews = contentManager.GetReviews();
        //    Review savedReview = reviews.FirstOrDefault(r => r.Guid == guid);

        //    // Assert
        //    Assert.IsFalse(result);
        //    Assert.IsNull(savedReview);
        //    Assert.IsTrue(mockFileSystem.File.Exists(path));
        //    Assert.AreEqual(itemCount, reviews.Count);
        //    Assert.AreEqual(FileStrings.GetFile(fileName), mockFileSystem.FileSystem.File.ReadAllText(path));
        //}

        [Test]
        [TestCase("oneReview", "30394e3f-e9bc-44ba-a889-0c9d781f93ff", 1)]
        [TestCase("twoReviews", "7176da9a-3670-4fe3-8d11-cb19d697620e", 2)]
        [TestCase("tenReviews", "3b5e6356-394f-45dd-b10a-3cb5466720f7", 10)]
        public async Task UpsertReviewShouldUpdateReviewInFileIfExistsWithSameReview(string fileName, string guidStr, int itemCount)
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            string path = $@"C:\tmp\{fileName}.json";
            var mockInputFile = new MockFileData(FileStrings.GetFile(fileName));
            mockFileSystem.AddFile(path, mockInputFile);
            IAsyncContentManager contentManager = new FileContentManager(path, mockFileSystem);
            Guid guid = new(guidStr);
            Review reviewToSave = new()
            {
                Guid = guid,
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Archived,
                Records = new List<Record>()
            };

            // Act
            await contentManager.UpsertReviewAsync(reviewToSave);
            List<Review> reviews = await contentManager.GetReviewsAsync();
            Review savedReview = reviews.FirstOrDefault(r => r.Guid == guid);

            // Assert
            Assert.IsNotNull(savedReview);
            Assert.IsTrue(mockFileSystem.File.Exists(path));
            Assert.AreEqual(itemCount, reviews.Count);
            Assert.AreEqual(reviewToSave, savedReview);
        }
    }
}