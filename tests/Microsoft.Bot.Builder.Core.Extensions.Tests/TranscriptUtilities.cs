﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Core.Extensions.Tests
{
    /// <summary>
    /// Helpers to get activities from trancript files
    /// </summary>
    public static class TranscriptUtilities
    {
        /// <summary>
        /// Loads a list of activities from a transcript file.
        /// Use the context of the test to find the transcript file
        /// </summary>
        /// <param name="context">Test context</param>
        /// <returns>A list of activities to test</returns>
        public static IEnumerable<IActivity> GetFromTestContext(TestContext context)
        {
            var transcriptsRootFolder = TestUtilities.GetKey("TranscriptsRootFolder") ?? @"..\..\..\..\..\transcripts";
            var directory = Path.Combine(transcriptsRootFolder, context.FullyQualifiedTestClassName.Split('.').Last());
            var fileName = $"{context.TestName}.transcript";
            var path = Path.Combine(directory, fileName);
            if (!File.Exists(path))
            {
                Assert.Fail($"Required transcript file '{path}' does not exists in '{transcriptsRootFolder}' folder. Review the 'TranscriptsRootFolder' environment variable value.");
            }

            var content = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Activity[]>(content);
        }

        /// <summary>
        /// Get a conversation reference.
        /// This method can be used to set the conversation reference needed to create a <see cref="Adapters.TestAdapter"/>
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>A valid conversation reference to the activity provides</returns>
        public static ConversationReference GetConversationReference(this IActivity activity)
        {
            bool IsReply(IActivity act) => string.Equals("bot", act.From?.Role, StringComparison.InvariantCultureIgnoreCase);
            var bot = IsReply(activity) ? activity.From : activity.Recipient;
            var user = IsReply(activity) ? activity.Recipient : activity.From;
            return new ConversationReference
            {
                User = user,
                Bot = bot,
                Conversation = activity.Conversation,
                ChannelId = activity.ChannelId,
                ServiceUrl = activity.ServiceUrl
            };
        }
    }
}
