using LastWeek.Model.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LastWeek.Model.Tests
{
    [TestFixture]
    public class ReviewTests
    {
        private class FakeEntry : Record { }

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
                Records = new List<Record>()
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
                Records = new List<Record>()
            };
            object notAReview = new object();

            // Act
            bool areEqual = firstReview.Equals(notAReview);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void EqualsReviewWithDifferentGuidReturnsFalse()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };
            Review secondReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Records = firstReview.Records
            };

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void EqualsReviewWithDifferentEndDateReturnsFalse()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate.AddDays(-1),
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Records = firstReview.Records
            };

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void EqualsReviewWithDifferentStartDateReturnsFalse()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate.AddDays(-1),
                Status = firstReview.Status,
                Records = firstReview.Records
            };

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void EqualsReviewWithDifferentStatusReturnsFalse()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = ReviewStatus.Validated,
                Records = firstReview.Records
            };

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void EqualsSameReviewWithNullEntriesCountReturnsTrue()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = null
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Records = null
            };

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void EqualsSameReviewWithDifferentEntriesCountReturnsFalse()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Records = new List<Record>() { new RangeRecord() }
            };

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        [TestCase(1, new int[] { 0 })]
        [TestCase(2, new int[] { 1 })]
        [TestCase(10, new int[] { 3 })]
        [TestCase(10, new int[] { 5, 7 })]
        public void EqualsSameReviewWithDifferentEntriesReturnsFalse(int listLength, int[] difIds)
        {
            // Arrange
            List<Record> firstList = new();
            List<Record> secondList = new();
            for (int i = 0; i < listLength; i++)
            {
                if (Array.IndexOf(difIds, i) != -1)
                {
                    firstList.Add(new FakeEntry());
                    secondList.Add(new FakeEntry());
                }
                else
                {
                    Record fake = new FakeEntry();
                    firstList.Add(fake);
                    secondList.Add(fake);
                }

            }
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = firstList
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Records = secondList
            };

            // Act
            bool areEqual = firstReview.Equals(secondReview);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void UpdateShouldChangeAllProperties()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = DateTime.Today,
                StartDate = DateTime.MinValue,
                Status = ReviewStatus.Validated,
                Records = new List<Record>() { new RangeRecord() }
            };

            // Act
            bool result = firstReview.Update(secondReview);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(firstReview, secondReview);
        }

        [Test]
        public void UpdateShouldChangeNothingIfDifferentGuid()
        {
            // Arrange
            Review firstReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                Status = ReviewStatus.Active,
                Records = new List<Record>()
            };
            Review secondReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.Today,
                StartDate = DateTime.MinValue,
                Status = ReviewStatus.Validated,
                Records = new List<Record>() { new RangeRecord() }
            };

            // Act
            bool result = firstReview.Update(secondReview);

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(firstReview, secondReview);
        }
    }
}