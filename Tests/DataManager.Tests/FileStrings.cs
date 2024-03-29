﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Tests
{
    public static class FileStrings
    {
        public static string GetFile(string fileName)
        {
            return typeof(FileStrings).GetField(fileName).GetValue(null) as string;
        }


        public const string empty = "";
        public const string oneReview = @"[
  {
    ""Guid"": ""30394e3f-e9bc-44ba-a889-0c9d781f93ff"",
    ""Status"": 0,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
    }
]";
        public const string twoReviews = @"[
  {
    ""Guid"": ""7176da9a-3670-4fe3-8d11-cb19d697620e"",
    ""Status"": 0,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""f92c9883-a2cf-4adb-bb69-f34b6ab29aad"",
    ""Status"": 1,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  }
]";
        public const string tenReviews = @"[
  {
    ""Guid"": ""7176da9a-3670-4fe3-8d11-cb19d697620e"",
    ""Status"": 0,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""f92c9883-a2cf-4adb-bb69-f34b6ab29aad"",
    ""Status"": 1,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""70dc9b97-28ba-489f-b4c8-6c27675d52a5"",
    ""Status"": 2,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""ca2b79f4-daec-4abd-9ab2-c67891aa7763"",
    ""Status"": 0,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""3b5e6356-394f-45dd-b10a-3cb5466720f7"",
    ""Status"": 1,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""1e2c617c-5e23-41a4-bc32-c2d8e73c683f"",
    ""Status"": 2,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""baafced7-4824-4c30-badf-bec20bcd05e4"",
    ""Status"": 0,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""64941463-e6d1-4053-8619-9ba6b2882154"",
    ""Status"": 1,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""5efc0d4d-189b-4a25-aed9-1b53625cf664"",
    ""Status"": 2,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""c4671b48-216f-470d-a447-675ece9da96c"",
    ""Status"": 0,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  }
]";
        public const string doubleReviews = @"[
  {
    ""Guid"": ""7176da9a-3670-4fe3-8d11-cb19d697620e"",
    ""Status"": 0,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  },
  {
    ""Guid"": ""7176da9a-3670-4fe3-8d11-cb19d697620e"",
    ""Status"": 0,
    ""StartDate"": ""2009-02-15T00:00:00Z"",
    ""EndDate"": ""2009-02-15T00:00:00Z"",
    ""Entries"": []
  }
]";
    }
}
