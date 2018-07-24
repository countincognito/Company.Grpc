using Grpc.Core;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Zametek.Utility;

namespace Company.Grpc.Common
{
    public class TrackingHelper
    {
        private const string c_InvalidHeaderKeyCharacters = @"[^a-z0-9_-]";
        private static readonly string s_TrackingContextKeyName;

        static TrackingHelper()
        {
            s_TrackingContextKeyName = string.Concat(
                Regex.Replace(TrackingContext.FullName.ToLowerInvariant(), c_InvalidHeaderKeyCharacters, "-"),
                Metadata.BinaryHeaderSuffix);
        }

        public static Metadata ProcessHeaders(Metadata headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            Metadata.Entry trackingEntry = headers.FirstOrDefault(x => string.CompareOrdinal(x.Key, s_TrackingContextKeyName) == 0);

            // Retrieve the tracking context from the message header, if it exists.
            if (trackingEntry != null)
            {
                // If an tracking context exists in the message header, always use it to replace the ambient context.
                TrackingContext tc = TrackingContext.DeSerialize(trackingEntry.ValueBytes);
                tc.SetAsCurrent();
            }
            else
            {
                // If no tracking context exists then create one.
                TrackingContext.NewCurrentIfEmpty();

                Debug.Assert(TrackingContext.Current != null);

                // Copy the tracking context to the message header.
                byte[] byteArray = TrackingContext.Serialize(TrackingContext.Current);
                headers.Add(s_TrackingContextKeyName, byteArray);
            }

            return headers;
        }
    }
}
