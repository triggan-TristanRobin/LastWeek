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
    }
}