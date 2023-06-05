using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dating_Application
{
    public class Profile
    {
        private string name;
        private DateTime birthday;
        private string[] interests;
        protected double Elo { get; private set; }
        private bool godMode;
        public Profile(string name, DateTime birthday, string[] interests)
        {
            this.name = name;
            this.birthday = birthday;
            this.interests = interests;
            setElo(50.0);
        }



        // getters
        public double getElo()
        {
            return Elo;
        }
        public string getName()
        {
            return name;
        }
        public int getAge()
        {
            return birthday.Year - DateTime.UtcNow.Year;
        }
        public string[] getInterests()
        {
            return interests;
        }

        public void incrementElo(Profile profile)
        {
            // incrementing elo is stronger than decrementing elo due to the fact more people are disliking than liking on any given profile
            // this function ADDS the weighted elo of the liking person to the current profile
            this.setElo(this.getElo() + profile.getElo()/ 50.0);
        }
        public void decrementElo(Profile profile)
        {
            // decrementing is weaker than incrementing (vice versa)
            // this function SUBTRACTS the weighted elo of the liking person to the current profile
            this.setElo(this.getElo() - profile.getElo() / 50.0);
        }

        protected void setElo(double elo)
        {
            this.Elo = elo;
        }
    }
    public class AdminProfile : Profile
    {
        public AdminProfile(string name, DateTime birthday, string[] interests) : base(name, birthday, interests)
        {
            
        }

        public void adminSetElo(double elo)
        {
            setElo(elo);
        }
    }
}
