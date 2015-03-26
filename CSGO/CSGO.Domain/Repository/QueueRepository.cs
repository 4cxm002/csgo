using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO.Domain.Repository
{
    public class QueueRepository
    {
        public QueueRepository()
        {

        }

        public void RetrieveAll()
        {
            //TODO: Retrieve all specific queues
        }

        public void Retrieve(int id)
        {
            //TODO: Retrieve specific queue based off of ID
        }

        public void AddToQueue(object player)
        {
            //TODO: Add the specific player to a queue
        }

        public void DetermineWinner(int id)
        {
            //TODO: Determine the winner of the specified queue based off of ID
        }
    }
}
