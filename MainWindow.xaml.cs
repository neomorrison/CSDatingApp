using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Dating_Application.Profile;

namespace Dating_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int currentIndex;
        List<Profile> profiles;
        List<Profile> seenProfiles;

        AdminProfile userProfile;


        public MainWindow()
        {
            InitializeComponent();

            profiles = new List<Profile>();

            // this creates the user profile for the active user. in practice this would be an actual profile but for this example it will be basic just so other parts function
            userProfile = new AdminProfile("USER", DateTime.Now, new string[] { "this", "wont", "matter" });
            userProfile.adminSetElo(500.0); // manually set own elo to see effects of weighted elo // default value is 50

            // generates profiles to be added
            populateProfiles();


            if(profiles.Count > 0 ) // if profiles list is valid
            {
                currentIndex = -1;
                showNextProfile();
            }
        }

        private void populateProfiles()
        {
            // this generates random profiles
            // it will generate as many profiles as there are names in the names list

            List<string> names = new List<string> { "randy", "joe", "alex", "emily", "carly", "jen", "kyle", "demarcus", "jamal", "serena", "megan" };
            List<DateTime> birthdays = new List<DateTime>();
            List < string[]> interests = new List<string[]>();
            string[] interestPossibilities = new string[] { "football", "movies", "eating out", "staying in", "going out", "date night", "night life", "soccer", "baseball", "sports", "gym", "hockey", "video games", "traveling", "hiking", "climbing", "dancing", "concerts" };
            for(int i = 0; i < names.Count; i++)
            {
                Random rnd = new Random();
                int randomYear = rnd.Next(1970, 2005);
                int randomMonth = rnd.Next(1, 13);
                int randomDay = rnd.Next(1, 30);
                DateTime randDate = new DateTime(randomYear, randomMonth, randomDay);
                birthdays.Add(randDate);

                // generate up to 10 random interests
                int howManyInterests = rnd.Next(1, 11);
                int randInterest = rnd.Next(0, interestPossibilities.Length-1);
                string[] profileInterests = new string[howManyInterests];
                List<int> seenInterests = new List<int>();
                for(int x = 0; x < howManyInterests; x++)
                {
                    profileInterests[x] = interestPossibilities[randInterest];
                    seenInterests.Add(randInterest);
                    while (seenInterests.Contains(randInterest))
                    {
                        randInterest = rnd.Next(0, interestPossibilities.Length - 1);
                    }
                    
                }
                interests.Add(profileInterests);

                Profile addedProfile = new Profile(names[i], birthdays[i], interests[i]);
                profiles.Add(addedProfile);
            }
        }

        private void showNextProfile()
        {
            currentIndex++;
            // Get the profile of the current index
            Profile instance = profiles[currentIndex];

            // Set profile information to that of the object
            namelabel.Content = instance.getName();

            string[] instanceInterests = instance.getInterests();

            interests.Text = "";

            for(int i = 0; i < instanceInterests.Length; i++)
            {
                interests.Text += instanceInterests[i];
                if(i != instanceInterests.Length - 1)
                {
                    interests.Text += ", ";
                }
            }

            // admin stats
            profileelo.Content = "Profile elo: " + instance.getElo();
            likedcount.Content = "Total likes: " + likedamount;
            dislikedcount.Content = "Total dislikes: " + dislikedamount;
            superlikesamount.Content = "Superlikes left: " + superlikesleft;
        }
        int likedamount = 0;
        int dislikedamount = 0;
        int superlikesleft = 2; // amount of superlikes available to the user in control

        private void addProfile(string name, DateTime time, string[] interests)
        {
            Profile profile = new Profile(name, time, interests);
            profiles.Add(profile);
        }

        // like button
        private void likebutton_Click(object sender, RoutedEventArgs e)
        {
            Profile instance = profiles[currentIndex];
            likedamount++;
            if (profiles.Count > currentIndex + 1)
            {
                instance.incrementElo(userProfile);
                showNextProfile();
            }
            else if (showInfinite)
            {
                currentIndex = -1;
                showNextProfile();
                instance.incrementElo(userProfile);
            }
            else
            {
                instance.incrementElo(userProfile);
                namelabel.Content = "Ran out of profiles";
                interests.Text = "";
            }
        }

        // super like has 2X weight
        private void superlikebutton_Click(object sender, RoutedEventArgs e)
        {
            if(superlikesleft > 0)
            {
                Profile instance = profiles[currentIndex];
                likedamount++;
                if (profiles.Count > currentIndex + 1)
                {
                    instance.incrementElo(userProfile);
                    instance.incrementElo(userProfile);
                    showNextProfile();
                }
                else if (showInfinite)
                {
                    currentIndex = -1;
                    showNextProfile();
                    instance.incrementElo(userProfile);
                    instance.incrementElo(userProfile);
                }
                else
                {
                    instance.incrementElo(userProfile);
                    instance.incrementElo(userProfile);
                    namelabel.Content = "Ran out of profiles";
                    interests.Text = "";
                }
                superlikesleft--;
            }
            else
            {
                superlikebutton.IsEnabled = false;
            }

        }

        // dislike button
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Profile instance = profiles[currentIndex];
            dislikedamount++;
            if (profiles.Count > currentIndex + 1)
            { 
                instance.decrementElo(userProfile);
                showNextProfile();
            }
            else if (showInfinite)
            {
                currentIndex = -1;
                showNextProfile();
                instance.decrementElo(userProfile);
            }
            else
            {
                instance.decrementElo(userProfile);
                namelabel.Content = "Ran out of profiles";
                interests.Text = "";

            }
        }

        bool showInfinite = false;

        private void infiniteProfiles_Checked(object sender, RoutedEventArgs e)
        {
            if(infiniteProfiles.IsChecked== true)
            {
                showInfinite = true;
            }
            else
            {
                showInfinite = false;
            }
        }
    }
}
