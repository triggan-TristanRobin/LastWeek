using Model;
using Moq;
using DataManager;
using NUnit.Framework;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DataManagerTests
{
    [TestFixture]
    public class ContentManagerTests
    {
        public IContentManager ContentManager { get; set; }
        private string path;

        [Test]
        public void GetReviewsShouldReturnEmptyListIfFileNotExists()
        {
            // Arrange
            string path = Path.Combine(Environment.CurrentDirectory, "data", "donotexist.json");
            ContentManager contentManager = new ContentManager(path);

            // Act
            List<Review> reviews = contentManager.GetReviews();

            // Assert
            Assert.IsFalse(reviews.Any());
        }

        [Test]
        public void GetReviewsShouldReturnEmptyListIfFileIsEmpty()
        {
            // Arrange
            string path = Path.Combine(Environment.CurrentDirectory, "data", "empty.json");
            ContentManager contentManager = new ContentManager(path);

            // Act
            List<Review> reviews = contentManager.GetReviews();

            // Assert
            Assert.IsFalse(reviews.Any());
        }

        [Test]
        [TestCase("oneReview", 1)]
        [TestCase("twoReviews", 2)]
        [TestCase("tenReviews", 10)]
        public void GetReviewsShouldReturnStoredListIfFileIsCorrect(string fileName, int expectedCount)
        {
            // Arrange
            string path = Path.Combine(Environment.CurrentDirectory, "data", $"{fileName}.json");
            ContentManager contentManager = new ContentManager(path);

            // Act
            List<Review> reviews = contentManager.GetReviews();

            // Assert
            Assert.AreEqual(expectedCount, reviews.Count());
        }

        [Test]
        public void GetReviewShouldReturnNullIfFileNotExists()
        {
            // Arrange
            string path = Path.Combine(Environment.CurrentDirectory, "data", "donotexist.json");
            ContentManager contentManager = new ContentManager(path);

            // Act
            Review review = contentManager.GetReview(It.IsAny<Guid>());

            // Assert
            Assert.IsNull(review);
        }

        [Test]
        public void GetReviewShouldReturnNullIfFileIsEmpty()
        {
            // Arrange
            string path = Path.Combine(Environment.CurrentDirectory, "data", "empty.json");
            ContentManager contentManager = new ContentManager(path);

            // Act
            Review review = contentManager.GetReview(It.IsAny<Guid>());

            // Assert
            Assert.IsNull(review);
        }

        [Test]
        [TestCase("oneReview", "30394e3f-e9bc-44ba-a889-0c9d781f93ff", ReviewStatus.Active)]
        [TestCase("twoReviews", "f92c9883-a2cf-4adb-bb69-f34b6ab29aad", ReviewStatus.Validated)]
        [TestCase("tenReviews", "1e2c617c-5e23-41a4-bc32-c2d8e73c683f", ReviewStatus.Archived)]
        [TestCase("tenReviews", "64941463-e6d1-4053-8619-9ba6b2882154", ReviewStatus.Validated)]
        public void GetReviewShouldReturnReviewIfOnlyMatch(string fileName, string guidStr, ReviewStatus expectedStatus)
        {
            // Arrange
            string path = Path.Combine(Environment.CurrentDirectory, "data", $"{fileName}.json");
            ContentManager contentManager = new ContentManager(path);
            Guid guid = new Guid(guidStr);

            // Act
            Review review = contentManager.GetReview(guid);

            // Assert
            Assert.AreEqual(guid, review.Guid);
            Assert.AreEqual(expectedStatus, review.Status);
            Assert.AreEqual(new DateTime(2009, 02, 15), review.StartDate);
            Assert.AreEqual(new DateTime(2009, 02, 15), review.EndDate);
            Assert.IsNotNull(review.Entries);
            Assert.IsFalse(review.Entries.Any());
        }

        [Test]
        public void GetReviewShouldtThrowExceptionIfMultipleMatches()
        {
            // Arrange
            string path = Path.Combine(Environment.CurrentDirectory, "data", "doubleReviews.json");
            ContentManager contentManager = new ContentManager(path);
            Guid guid = new Guid("7176da9a-3670-4fe3-8d11-cb19d697620e");

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => contentManager.GetReview(guid));
        }

        [Test]
        public void GetReviewShouldThrowExceptionWhenNoMatch()
        {
            // Arrange
            string path = Path.Combine(Environment.CurrentDirectory, "data", "oneReview.json");
            ContentManager contentManager = new ContentManager(path);
            Guid guid = new Guid("7176da9a-3670-4fe3-8d11-cb19d697620e");

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => contentManager.GetReview(guid));
        }
    }
}