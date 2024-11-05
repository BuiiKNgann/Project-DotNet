using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKS.Model
{
    internal class AllCustomerDetailsModel
    {
        public int cid { get; set; }
        public string cname { get; set; }
        public long mobile { get; set; }
        public string nationality { get; set; }
        public string gender { get; set; }
        public DateTime dob { get; set; }
        public string idproof { get; set; }
        public string address { get; set; }
        public DateTime checkin { get; set; }
        public DateTime? checkout { get; set; } // Nullable nếu có thể không có giá trị
        public int roomid { get; set; }
        public string checkoutStatus { get; set; }
    }
}
