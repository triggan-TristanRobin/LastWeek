using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Model.Tests
{
    [TestFixture]
    public class ReviewTests
    {
        [Test]
        public void EqualsSameObjectReturnsTrue()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Entries = new List<Entry>()
            };
            Review secondReview = firstReview;

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void EqualsNotAReviewReturnsFalse()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Entries = new List<Entry>()
            };
            object notAReview = new object();

            // Act
            bool areEqual = firstReview.Equals(notAReview);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void EqualsNotReviewWithDifferentGuidReturnsFalse()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Entries = new List<Entry>()
            };
            Review secondReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Entries = firstReview.Entries
            };

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsFalse(areEqual);
        }
    }
}