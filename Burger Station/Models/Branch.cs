using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Burger_Station.Models
{
    public enum District
    {
        North,
        South,
        Center,
    }

    public enum City
    {
        Ashdod,
        Ashkelon,
        Bat_Yam,
        Beersheba,
        Eilat,
        Givatayim,
        Haifa,
        Herzliya,
        Hod_HaSharon,
        Holo,
        Jerusalem,
        Kfar_Saba,
        Ness_Ziona,
        Netanya,
        Petah_Tikva,
        Raanana,
        Ramat_Gan,
        Ramat_HaSharon,
        Rehovot,
        Rishon_LeZion,
        Tel_Aviv,
        Yafo
    }

    public class Branch
    {
        public int Id { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9 ][a-zA-Z0-9 ]+")]
        public String Address { get; set; }

        public City City { get; set; }

        public District District { get; set; }

        public ICollection<BranchItem> BranchItems { get; set; }
    }
}
