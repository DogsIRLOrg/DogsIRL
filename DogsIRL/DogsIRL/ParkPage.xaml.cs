using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DogsIRL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DogsIRL
{
    public partial class ParkPage : ContentPage
    {
        public PetCard OtherDog { get; set; }

        public ParkPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            OtherDog = await GetRandomOtherDog(App.Username);
            OtherDogName.Text = OtherDog.Name;

            CurrentDogName.Text = App.CurrentDog.Name;
            GetInteraction(App.CurrentDog, OtherDog);
        }

        public async Task<PetCard> GetRandomOtherDog(string owner)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"{App.ApiUrl}/petcards");
            var pets = JsonConvert.DeserializeObject<List<PetCard>>(response);
            var otherPets = pets.Where(pet => pet.Owner != owner).ToList();

            int randomPetIndex = RandomNumber(0, otherPets.Count);
            
            return otherPets[randomPetIndex];
        }

        int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public void GetInteraction(PetCard currentDog, PetCard otherDog)
        {
            string[,] HelloConvo = new string[,]
            {
                {"Say what you feel and be who you are.", $"Why is that {otherDog.Name}?", "Because those who mind don\'t matter and those who matter don\'t mind." },
                {"Good morning, friend, what\'d you have for breakfast today?", "Eggs and bacon. What about you ?", "That\'s sounds like a healthy meal for a dog. I had just dog food, which is good enough for me." },
                {$"What are you up to today, {otherDog.Name}?", "Just snooping around the park. What\'s up with you?", "West coast living, walking without a leash mostly."},
                {"Hello there, aren\'t you looking more fluffy than usual. New haircut?", $"That\'s right, I like to keep it looking fresh. You got to, ya know {currentDog.Name}?", "Cool. Well that\'s just all right."},
                {$"The weather forecast shows chance of showers. How\'s that fare, {otherDog.Name}?", "Well that works great for me. I heard that wet dog smell\'s coming back.", "That\'s what\'s up. Wet dog style\'s coming back!"},
                {"I\'m glad I\'m a dog.", $"Me too, {currentDog.Name}. {currentDog.Name} wags their tail.", $"{otherDog.Name} wags their tail too."},
                {"I wonder if we are brave.", $"We sure look dog\'gone brave, {currentDog.Name}", "... but are we?"},
                {"This whole park is covered in bushes and trees.", $"That\'s right, {currentDog.Name}. And not one of them is mine.", "Well I do think that one is mine now."},
                {"You are in pretty good shape.", "I guess, for the shape I\'m in.", $"Never sell yourself short, {otherDog.Name}"},
                {"It feels more open here in the park.", $"I\'d say so, {currentDog.Name}. I\'d bark to that.", "Isn\'t it great, to be in the wide open air ?"},
                {"Today was good and well.", "Today was dandy and fun.", "Tomorrow\'s going to be another one."},
                {"Is there a place for us in this world?", "You mean like this dog park?", "That\'s good enough for me, if that\'s good enough for you."},
                {$"{currentDog.Name} sits, lays down, rolls over.", $"{otherDog.Name} barks, \"Bark\", and lays down too.", $"{currentDog.Name} barks back \"Bark\"."},
                {$"With the sun high in they sky, {currentDog.Name} looks up and relives their favorite summertime memories.", $"{otherDog.Name} patiently waits for the right time to speak up... \"everything okay, {currentDog.Name}?\"", "Couldn\'t be better. Wag more, bark less, ya know ?"},
                {$"{currentDog.Name} smells around the tall grass for something brand new.", $"{otherDog.Name} thinks they can smell it, too.", $"{currentDog.Name} finds what {otherDog.Name} thought they smelt."},
                {"All the dogs in the park are running after a squirel that\'s far too fast.", $"{otherDog.Name} waits for {currentDog.Name} to join in.", $"{currentDog.Name} waits for {otherDog.Name} to make their move."},
                {$"{currentDog.Name} thinks {otherDog.Name} has a great amount of fluff going on today.", $"{otherDog.Name} sees a mud puddle and jumps in.", $"{currentDog.Name} jumps in and joins in on the fun."},
                {$"{currentDog.Name} is asked to sit and stay. {otherDog.Name} overhears the command and knows what\'s coming.", $"{otherDog.Name} sits and stays first.", $"{currentDog.Name} sits and stays, too, and everyone gets treats."},
                {$"{currentDog.Name} wonders if {otherDog.Name} wants to snuggle buggle.", $"{otherDog.Name} appreciates that {currentDog.Name} has asked politely, and gives a big hug.", $"A big hug from {otherDog.Name} has made {currentDog.Name}\'s day. Snuggle buddies!"},
                {$"{currentDog.Name} sees a bald eagle in the sky, and hears its piercing squak.", $"{otherDog.Name} wonders what its like to fly like that bird in the sky.", $"{currentDog.Name} says, \"Look, the bird of our nation. Its come to give us good luck!\""},
                {$"All the money in the world couldn\'t keep {currentDog.Name} away from chasing the squirrel around the park.", $"{otherDog.Name} hasn\'t an ounce of will power and chases the critter.", $"Even though the squirrel is far too fast for any dog to catch, {currentDog.Name} joins {otherDog.Name} in the chase."},
                {$"{currentDog.Name} is glad they\'re a dog.", $"{otherDog.Name} is glad, too, and wags their tail.", $"{currentDog.Name} wags their tail, too."},
                {$"{currentDog.Name} takes notice of {otherDog.Name}'\'s resounding bark.", "\"Bark, bark, bark... Whimper, bark, yelp, yelp\"", "Maybe is not so resounding after all. \"Woof, woof\""},
                {$"{currentDog.Name} wags its tail.", $"{otherDog.Name} sniffs for something in the field.", $"{currentDog.Name} follows {otherDog.Name}; they want to join in on the fun."},
                {$"{currentDog.Name} is hungry for some food.", $"{otherDog.Name} wanders around looking for its tail.", $"{currentDog.Name} rolls around in the grass."},
                {$"{currentDog.Name} sees a cloud in the sky that looks like a cat.", $"Meanwhile, {otherDog.Name} leaps forward and says, \"do you see what I see?\"", "\"You don\'t like giant cats either?\" And then jumps up, after {otherDog.Name}... \"Squirrel!\""},
                {$"With great balance, {currentDog.Name} sits and focuses on the ball about to be thrown.", $"Meanwhile, {otherDog.Name} can hardly hold still.", $"In one giant leap, {currentDog.Name} catches the ball mid air, mid flight."},
                {$"{currentDog.Name} thinks and wonders.", $"{otherDog.Name} wonders and thinks", $"{currentDog.Name} thinks that as soon as things start happening, they\'ll start happening, too."},
                {$"{currentDog.Name} frollucks over the grassy field. And has never been more happy.", $"{otherDog.Name} stews in the great moment. Today is {currentDog.Name}\'s day.", $"{currentDog.Name} looks back at {otherDog.Name} and thinks outloud, {otherDog.Name} is A-okay!"}
            };

            string[,] Goodbye = new string[,]
            {
                {$"What a pleasure it was to meet with you.", "We should do this again soon. Bye!"},
                {"Well its nice to meet with you, I had so much fun.", "Maybe I\'ll see you around the park. Chao!"},
                {"I\'m glad you\'re doing well. Take good care of yourself.", $"See you next time, {otherDog.Name}."},
                {"What a pleasure is was to meet with you.", "Let\'s do this again soon. Bye!"},
                {"When we come back to the park, it\'d be great to see you again.", $"Awesome, possum. {otherDog.Name}That sounds swell."},
                {"Playing with you is as good as it gets.", $"Thanks, {otherDog.Name}, I enjoyed our time, too. Tata!"},
                {$"There\'s no other way to put it. {currentDog.Name}, you are a great friend.", $"Thanks, {otherDog.Name}! I sure do like you, too."},
                {"Even when you\'re down, know that I\'ll be around.", "That\'s great.Know, you\'ve got a friend in me, too."},
                {"I like you better than dog food, but not chicken casserole.", $"That\'s so nice.Thanks, {otherDog.Name}. Chicken casserole\'s the best."},
                {$"I think we are quite the pair, me and you, {currentDog.Name}.", "Its funny you say that. I was thinking the same thing. Bye!"},
                {"We\'ve got to get going. I\'ve had so much fun.", $"Same here, {otherDog.Name}. And more to come. See you around!"},
                {"Well, let\'s do this again some time.", $"That\'d be great, {otherDog.Name}. I\'d love to be your friend."},
                {"Take it from me pal, you\'ve been swell.", "See you around next time. Tata for now."},
                {"Do you think we can do this again sometime?", "I\'ll tell you what, next time I\'m at the park I\'ll bark your way."},
                {"Any chance at seeing you around again?", "If you need me, I\'ll come running.Chao!"},
                {"This has been a memorable day.", "Great to spend it with you. See ya next time."},
                {$"Okie dokie artichokie. I\'ve got to get going, {currentDog.Name}.", "You are awesome. I\'m looking forward to seeing you next time."},
                {$"{otherDog.Name} sees it is time to go, and obediently returns to their owner", $"{currentDog.Name} seems sad to see {otherDog.Name} leave, but knows that they are never too far away."}

            };

            int resultHello = RandomNumber(0, HelloConvo.GetLength(0));
            int resultGoodbye = RandomNumber(0, Goodbye.GetLength(0));

            LineOne.Text = HelloConvo[resultHello, 0];
            LineTwo.Text = HelloConvo[resultHello, 1];
            LineThree.Text = HelloConvo[resultHello, 2];
            LineFour.Text = Goodbye[resultGoodbye, 0];
            LineFive.Text = Goodbye[resultGoodbye, 1];


        }


        async void OnInteractClicked(System.Object sender, System.EventArgs e)
        {
            var previousPage = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new ParkPage());
            Navigation.RemovePage(previousPage);
     
        }

        async void OnCollectClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ProfileView());
        }
    }
}
