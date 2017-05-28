using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Kora.Models;

namespace Kora.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        readonly string[] validCommands = { "add", "list", "delete" };

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string text = activity.Text;
            string answer = string.Empty;

            //commands: add - list - complete
            if (text.StartsWith("/"))
            {
                string command = GetCommandFromText(text);
                if (IsValid(command))
                {
                    SaveConversationCommand(context, text);
                    GiveAnswer(command);
                }
                else
                {
                    answer = GetCommandNotIdentifiedAnswer(command);
                }
            }
            else
            {

            }


            // return our reply to the user
            await context.PostAsync(answer);
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }

        //TODO
        void SaveConversationCommand(IDialogContext context, string text)
        {

        }

        //TODO
        void GiveAnswer(IDialogContext context, string command)
        {

        }

        string GetCommandFromText(string text)
        {
            string command = string.Empty;
            if (text != null)
            {
                var peaces = text.Split(' ');
                command = peaces[0].TrimStart('/');
            }
            return command;
        }

        string GetCommandFromContext(IDialogContext context)
        {
            string command = context.ConversationData.GetValueOrDefault<string>("command");
            return command;
        }

        string GetCommandNotIdentifiedAnswer(string command)
        {
            return $"I don't know how to /{command}";
        }

        bool IsValid(string command)
        {
            return validCommands.FirstOrDefault(e => e == command) != null;
        }
    }
}