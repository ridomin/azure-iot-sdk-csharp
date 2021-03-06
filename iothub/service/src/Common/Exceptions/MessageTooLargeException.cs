// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Common.Exceptions
{
    [Serializable]
    public sealed class MessageTooLargeException : IotHubException
    {
        public MessageTooLargeException(int maximumMessageSizeInBytes)
            : this(maximumMessageSizeInBytes, string.Empty)
        {
        }

        public MessageTooLargeException(int maximumMessageSizeInBytes, string trackingId)
            : base("Message size cannot exceed {0} bytes".FormatInvariant(maximumMessageSizeInBytes), trackingId)
        {
            this.MaximumMessageSizeInBytes = maximumMessageSizeInBytes;
        }

        public MessageTooLargeException(string message)
            : base(message)
        {
        }

        public MessageTooLargeException(ErrorCode code, string message)
            : base(code, message)
        {
        }

        public MessageTooLargeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private MessageTooLargeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.MaximumMessageSizeInBytes = info.GetInt32("MaximumMessageSizeInBytes");
        }

        internal int MaximumMessageSizeInBytes
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("MaximumMessageSizeInBytes", this.MaximumMessageSizeInBytes);
        }
    }
}
