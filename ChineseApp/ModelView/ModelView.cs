using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ChineseApp.ModelViewNamespace
{
    public class ModelView
    {
        private ccDB db;
        public ObservableCollection<ccData> chardata
        { private set; get; }

        private static ModelView instance;
        public static ModelView Instance
        {
            get
            {
                if (instance == null)
                    instance = new ModelView();
                return instance;
            }
        }

        // Considering making this static because we should only have one
        public ModelView()
        {
            db = new ccDB("Data Source=isostore:/ChineseApp.sdf");

            if (db.DatabaseExists() == false)
                db.CreateDatabase();

            // Load the Data into an ObservableCollection
            var cData =
                from data in db.data
                select data;

            chardata = new ObservableCollection<ccData>(cData);
        }

        public void add(ccData c)
        {
            chardata.Add(c);
            db.data.InsertOnSubmit(c);
            db.SubmitChanges();
        }

        public void remove(int ID)
        {
            var idQuery =
                from ccdata in chardata
                where ID == ccdata.id
                select ccdata;

            foreach (ccData c in idQuery)
            {
                chardata.Remove(c);
                db.data.DeleteOnSubmit(c);
            }
            db.SubmitChanges();
        }

        public ccData retrieve(int ID)
        {
            if (ID >= chardata.Count)
                return null;
            return chardata.ElementAt(ID); // consider adding checking
        }

        public ccData getRandom()
        {
            Random r = new Random();
            int randID = r.Next(0, chardata.Count - 1);

            return chardata.ElementAt(randID);
        }

        public void reset()
        {
            if (db.DatabaseExists())
                db.DeleteDatabase();
            db.CreateDatabase();
        }
    }
}
