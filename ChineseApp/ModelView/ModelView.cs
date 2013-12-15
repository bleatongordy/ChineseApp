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
        private ObservableCollection<ccData> chardata;

        // Considering making this static because we should only have one
        public ModelView()
        {
            db = new ccDB("Data Source=isostore:/ChineseApp.sdf");

            if (db.DatabaseExists() == false)
                db.CreateDatabase();

            reset();

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

        public ObservableCollection<ccData> retrieveGroup(int chapternum, string bookname)
        {
            var gQuery =
                from gdata in chardata
                where chapternum == gdata.chapternum && bookname == gdata.bookname
                select gdata;

            return new ObservableCollection<ccData>(gQuery);
        }

        public ccData retrieve(int ID)
        {
            if (ID >= chardata.Count)
                return null;
            return chardata.ElementAt(ID); // consider adding checking
        }

        public void reset()
        {
            if (db.DatabaseExists())
                db.DeleteDatabase();
            db.CreateDatabase();
        }
    }
}
