using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BargainHunter.Models;
using Hangfire;
using Twilio;

namespace BargainHunter.Helpers
{
    public class TaskHelper
    {
        private static AmazonHelper _amazonHelper;
        private static TwilioRestClient _twilio;

        public TaskHelper(AmazonHelper amazonHelper)
        {
            _amazonHelper = amazonHelper;
            _twilio = new TwilioRestClient(Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"), Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN"));
        }

        private static void AddFetchTask(Deal deal)
        {
            RecurringJob.AddOrUpdate(deal.DealCode, () => TaskHelper.AddOrUpdateDeal(deal), Cron.Hourly);
        }

        private static bool IsCheaperDeal(Deal deal, Deal updatedDeal)
        {
            // check if the new price is lower than the old
            var isLower = updatedDeal.Price < deal.Price;
            return isLower;
        }

        private static void NotifyCheaperDeal(Deal deal, Deal updatedDeal)
        {
            var difference = deal.Price - updatedDeal.Price;
            var message = String.Format("The item {0} is now cheaper by {1:c}. Head to {2} to buy it.", updatedDeal.DealNick, difference, updatedDeal.Url);
            _twilio.SendMessage(Environment.GetEnvironmentVariable("TWILIO_NUMBER"), Environment.GetEnvironmentVariable("MY_NUMBER"), message);
        }

        public static Deal AddOrUpdateDeal(Deal deal)
        {
            var updatedDeal = (Deal)deal.Clone();
            updatedDeal.Price = _amazonHelper.GetPriceByAsin(deal.DealCode);

            using (var bhe = new BargainHunterEntities())
            {
                // Check if deal exists
                var databaseDeal = bhe.Deals.AsNoTracking().SingleOrDefault(x => x.DealCode == deal.DealCode);
                if (databaseDeal == null)
                {
                    // Create deal
                    bhe.Deals.Add(updatedDeal);

                    // Add a task
                    AddFetchTask(updatedDeal);
                }
                else
                {
                    // update the deal
                    bhe.Deals.Attach(updatedDeal);
                    bhe.Entry(updatedDeal).State = EntityState.Modified;

                    // Check to see if cheaper
                    if (IsCheaperDeal(databaseDeal, updatedDeal))
                    {
                        NotifyCheaperDeal(databaseDeal, updatedDeal);
                    }

                }
                bhe.SaveChanges();
            }
            return updatedDeal;
        }
    }
}