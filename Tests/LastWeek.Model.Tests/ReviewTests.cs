using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LastWeek.Model.Tests
{
    [TestFixture]
    public class ReviewTests
    {
        private class FakeEntry : Entry { }

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
        public void EqualsReviewWithDifferentGuidReturnsFalse()
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
                Entries = new List<Entry>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate.AddDays(-1),
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Entries = firstReview.Entries
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
                Entries = new List<Entry>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate.AddDays(-1),
                Status = firstReview.Status,
                Entries = firstReview.Entries
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
                Entries = new List<Entry>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = ReviewStatus.Validated,
                Entries = firstReview.Entries
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
                Entries = null
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Entries = null
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
                Entries = new List<Entry>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Entries = new List<Entry>() { new RangeEntry() }
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
            List<Entry> firstList = new();
            List<Entry> secondList = new();
            for (int i = 0; i < listLength; i++)
            {
                if (Array.IndexOf(difIds, i) != -1)
                {
                    firstList.Add(new FakeEntry());
                    secondList.Add(new FakeEntry());
                }
                else
                {
                    Entry fake = new FakeEntry();
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
                Entries = firstList
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = firstReview.EndDate,
                StartDate = firstReview.StartDate,
                Status = firstReview.Status,
                Entries = secondList
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
                Entries = new List<Entry>()
            };
            Review secondReview = new()
            {
                Guid = firstReview.Guid,
                EndDate = DateTime.Today,
                StartDate = DateTime.MinValue,
                Status = ReviewStatus.Validated,
                Entries = new List<Entry>() { new RangeEntry() }
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
                Entries = new List<Entry>()
            };
            Review secondReview = new()
            {
                Guid = Guid.NewGuid(),
                EndDate = DateTime.Today,
                StartDate = DateTime.MinValue,
                Status = ReviewStatus.Validated,
                Entries = new List<Entry>() { new RangeEntry() }
            };

            // Act
            bool result = firstReview.Update(secondReview);

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(firstReview, secondReview);
        }
    }
}