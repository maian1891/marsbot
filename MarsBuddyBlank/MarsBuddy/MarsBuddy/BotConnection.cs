﻿using Microsoft.Bot.Connector.DirectLine;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MarsBuddy
{
    class BotConnection
    {
        public DirectLineClient Client = new DirectLineClient("W9bU-qVptDs.cwA.Njw.NSTbChxu6Btkdj9Faat75tWT_ofGaNCcp3qy0_ygGf4");
        public Conversation MainConversation;
        public ChannelAccount Account;

        public BotConnection(string accountName)
        {
            MainConversation = Client.Conversations.StartConversation();
            Account = new ChannelAccount
            {
                Id = accountName,
                Name = accountName
            };
        }
        public void SendMessage(string message)
        {
            Activity activity = new Activity
            {
                From = Account, Text = message,
                Type = ActivityTypes.Message
            };

            Client.Conversations.PostActivity(MainConversation.ConversationId, activity);
        }

        public async Task GetMessagesAsync(ObservableCollection<MessageListItem> collection)
        {
            string watermark = null;
            //Loop retrieval    while(true)    {
            Debug.WriteLine("Reading message every 3 seconds");
            //Get activities (messages) after the watermark        
            var activitySet = await Client.Conversations.GetActivitiesAsync(MainConversation.ConversationId, watermark);

            //Set new watermark        
            watermark = activitySet?.Watermark;
            //Loop through the activities and add them to the list    
            foreach (Activity activity in activitySet.Activities)
            {
                if (activity.From.Name == "MarsBot")
                {
                    collection.Add(new MessageListItem(activity.Text, activity.From.Name));
                }
            }
            //Wait 3 seconds 
            await Task.Delay(3000);
        } 
        
    }

}
