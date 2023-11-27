using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RaffleNamespace
{
	public class Raffle
	{
		static double pot = 100;
		static List<User> users = new List<User>();
		static List<int> winningTicket;
		static int pricePerTicket = 5;
		static bool drawStatus = false;

		class Group
		{
			public int groupNumber { get; set; }

			public User users { get; set; }

			public int countOfUsers { get; set; }
		}
		class User
		{
			public string UserName { get; set; }
			public List<Ticket> Tickets { get; set; }

			public User(string userName)
			{
				UserName = userName;
				Tickets = new List<Ticket>();
			}
		}
		class Ticket
		{
			public List<int> Numbers { get; set; }

			public Ticket(List<int> numbers)
			{
				Numbers = numbers;
			}
		}

		static void Main()
		{
			Console.WriteLine("Welcome to My Raffle App");
			if (!drawStatus)
			{
				Console.WriteLine($"Status: Draw has not started");
			}
			else
			{
				Console.WriteLine($"Status: Draw is ongoing. Raffle pot size is {pot}");
			}

			while (true)
			{
				DisplayMainMenu();

				string choice = Console.ReadLine();

				switch (choice)
				{
					case "1":
						StartNewDraw();
						break;
					case "2":
						BuyTickets();
						break;
					case "3":
						RunRaffle();
						break;
					default:
						Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
						break;
				}
			}
		}
		static void DisplayMainMenu()
		{
			Console.WriteLine("\n[1] Start a New Draw");
			Console.WriteLine("[2] Buy Tickets");
			Console.WriteLine("[3] Run Raffle");
			Console.Write("Enter your choice: ");
		}
		static void StartNewDraw()
		{
			drawStatus = true;
			Console.WriteLine($"New Raffle draw has been started. Initial pot size: {pot}");
			Console.WriteLine("Press any key to return to main menu.");
		}
		static void BuyTickets()
		{
			Console.Write("Enter your name, number of tickets to purchase : ");
			string input = Console.ReadLine();

			string[] inputParts = input.Split(',');

			if (inputParts.Length == 2 && int.TryParse(inputParts[1], out int numTickets) && numTickets > 0)
			{
				string userName = inputParts[0];
				User user = new User(userName);
				for (int i = 0; i < numTickets && i < 5; i++)
				{
					Ticket ticket = new Ticket(GenerateRandomTicket());
					user.Tickets.Add(ticket);
				}

				users.Add(user);

				pot = pot + numTickets * pricePerTicket;

				Console.WriteLine($"Hi {userName}, you have purchased {numTickets} ticket (s) -");
				DisplayUserTickets(user);
			}
			else
			{
				Console.WriteLine("Invalid input. Please enter your name and a valid number of tickets.");
			}
		}
		static void DisplayUserTickets(User user)
		{
			foreach (var ticket in user.Tickets)
			{
				Console.WriteLine($"Ticket: {string.Join(" ", ticket.Numbers)}");
			}

			Console.WriteLine("Press any key to return to the main menu.");
			Console.ReadKey();
		}
		static void RunRaffle()
		{
			if (users.Count == 0)
			{
				Console.WriteLine("No users have purchased tickets. Cannot run the raffle.");
				return;
			}

			winningTicket = GenerateRandomTicket();

			Console.WriteLine("Running Raffle..");

			Console.WriteLine($"Winning Ticket is {string.Join(" ", winningTicket)} \n");

			GetWinningGroups(users);
			
			Console.WriteLine($"\nRemaining Pot: ${pot}");
			drawStatus = false;
		}
		public static List<int> GenerateRandomTicket()
		{
			Thread.Sleep(1000);
			Random random = new Random();
			List<int> numbers = new List<int>();

			for (int i = 0; i < 5; i++)
			{
				int number;
				do
				{
					number = random.Next(1, 16);
				} while (numbers.Contains(number));
				numbers.Add(number);
			}

			return numbers;
		}
		static void GetWinningGroups(List<User> users)
		{
			List<Group> groupsWithCount = new List<Group>();
			int totalCountPerUser;
			int matchingNumbers;

			for (var i = 2; i <= 5; i++)
			{
				foreach (var user in users)
				{
					totalCountPerUser = 0;
					matchingNumbers = 0;
					foreach (var ticket in user.Tickets)
					{
						matchingNumbers = CountMatchingNumbers(ticket.Numbers);

						if (matchingNumbers >= i)
						{
							totalCountPerUser++;
						}
					}
					groupsWithCount.Add(new Group { groupNumber = i, users = user, countOfUsers = totalCountPerUser });
				}
			}

			int totalDivisionsPerGroup;
			double perDivision;
			double rewardPercentage;
			List<Group> usersListTemp = new List<Group>();

			for (int i = 2; i <= 5; i++)
			{
				totalDivisionsPerGroup = 0;
				perDivision = 0;
				foreach (var group in groupsWithCount)
				{
					if (group.groupNumber == i)
						totalDivisionsPerGroup += group.countOfUsers;
				}

				rewardPercentage = GetRewardPercentage(i);
				perDivision = totalDivisionsPerGroup != 0 ? rewardPercentage / totalDivisionsPerGroup : 0;

				usersListTemp = groupsWithCount.FindAll(x => x.groupNumber == i).Select(x => x).ToList();

				DisplayResultsPerGroup(i, usersListTemp, perDivision);
			}
		}
		static void DisplayResultsPerGroup(int groupNumber, List<Group> usersListTemp, double perDiv)
		{
			Console.WriteLine($"Group {groupNumber} Winners :");

			foreach (var group in usersListTemp)
			{
				if (perDiv != 0)
					Console.WriteLine($"{group.users.UserName} with {group.countOfUsers} tickets(s) - {perDiv * group.countOfUsers}");
				
				pot -= perDiv * group.countOfUsers;
			}
			if (perDiv == 0)
			{
				Console.WriteLine("Nil");
			}
			Console.WriteLine($"\n");
		}
		public static double GetRewardPercentage(int groupNumber)
		{
			double rewards = 0;

			switch (groupNumber)
			{
				case 2:
					rewards = 0.10 * pot;
					break;
				case 3:
					rewards = 0.15 * pot;
					break;
				case 4:
					rewards = 0.25 * pot;
					break;
				case 5:
					rewards = 0.50 * pot;
					break;
			}
			return rewards;
		}
		static int CountMatchingNumbers(List<int> userNumbers)
		{
			return winningTicket.Intersect(userNumbers).Count();
		}
	}
}

