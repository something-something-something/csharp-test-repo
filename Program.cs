using System;
using System.Collections.Generic;
using System.Linq;

namespace csharp_test_repo
{
	class Program
	{
		static void Main(string[] args)
		{
			var invertList = new List<Invertabrate>() { };
			string command = "help";
			while (command != "exit")
			{
				switch (command)
				{
					case "help":
						PrintHeLp();
						break;
					case "list":
						PrintList(invertList);
					break;
					case "addfly":
						Console.Write("Species>");
						string species = Console.ReadLine();

						invertList.Add(new Fly(species, 6));
						break;
					case "rm":
						PrintList(invertList);
						if(invertList.Count>0){
							Console.WriteLine("Enter a number to get rid of");
							int rmNum=SelectFromList(invertList);
							invertList.RemoveAt(rmNum);
						}
						else{
							Console.WriteLine("Nothing to remove");
						}
					break;
					case "age":
						PrintList(invertList);
						if(invertList.Count>0){
							Console.WriteLine("Enter a number of the insect to increase the age");
							int ageNum=SelectFromList(invertList);
							invertList[ageNum].AdvanceToNextStage();
						}
						else{
							Console.WriteLine("Nothing to age");
						}
					
					break;
					case "fly":
						PrintList(invertList);
						if(invertList.Count>0){
							Console.WriteLine("Enter a number of the insect to try to get it to fly");
							int flySelect=SelectFromList(invertList);
							invertList[flySelect].Fly();
						}
						else{
							Console.WriteLine("Nothing to fly");
						}
					break;
					case "find":
						Console.WriteLine("Enter the exact species to find");
						string speciesToFind=Console.ReadLine();
						var l=invertList.Where((Invertabrate inv,int i)=>{
							return inv.Species==speciesToFind;
						}).ToList();
						PrintList(l);
					break;
					default:
						Console.WriteLine("Not RECOGNIZED");
						PrintHeLp();
						break;
				}



				Console.Write("Enter command or help for list of commands>");
				command = Console.ReadLine().ToLower().Trim();
			}

		}

		public static void PrintHeLp()
		{

			string helpText = @"
help	Prints This help Text
addfly	Adds a new Fly.
list	prints a list
rm	Removes an inverabrate
age	move insect to next stage
fly	try to get the invertabrate to fly
find	search by species name
exit	Exits program
			";
			Console.WriteLine(helpText);
		}

		public static void PrintList(List<Invertabrate> iL){
			string table="Number\tSpecies\tKind\tLifeStage\tLegs\n";
			for(int i=0;i<iL.Count;i++){
				table+=$"{i}\t{iL[i].InfoAsLine}\n";
			}
			Console.WriteLine(table);
		}

		public static int SelectFromList(List<Invertabrate> il){
			while(true){
				Console.Write("Select a Number>");
				string resp=Console.ReadLine();
				try{
					int choice=int.Parse(resp);
					if(choice<il.Count&&choice>=0){
						return choice;
					}
					else{
						Console.WriteLine($"Out of range chose between 0 and {il.Count-1}");
					}
				}
				catch(Exception e){
					Console.WriteLine("Input a valid number");
				}


			}

		}
	}

	public enum LIFESTAGE
	{
		EGG,
		LARVA,
		NYMPH,
		SPIDERLING,
		PUPA,
		ADULT

	}
	public interface Invertabrate
	{
		public int Legs { get; set; }
		string Species { get; }

		public bool CanFly { get; }

		public LIFESTAGE CurrentLifeStage { get; }

		public void AdvanceToNextStage()
		{
			LIFESTAGE past = CurrentLifeStage;
			LIFESTAGE curr = this.MoveLifeCycleForwards();
			Console.WriteLine($"{this.Species} has matured from {past} to a {curr}");
		}
		public string InfoAsLine{
			get{

				
				return $"{this.Species}\t{this.GetType().Name}\t{this.CurrentLifeStage}\t{this.Legs}";
			}
		}
		public LIFESTAGE MoveLifeCycleForwards();
		public void Fly()
		{
			if (CanFly)
			{
				Console.WriteLine($"{Species} is FLYING!");
			}
			else{
				Console.WriteLine($"{Species} can not curently fly!");
			}
		}
	}

	public class Insect : Invertabrate
	{
		public int Legs
		{
			get
			{
				return insectLegs;
			}
			set
			{
				if (value > 6)
				{
					throw new Exception("Insect can't have more than six legs");
				}
				this.insectLegs = value;
			}
		}
		public virtual bool CanFly { get { return false; } }
		public string Species { get; }
		private int insectLegs = 6;
		public virtual LIFESTAGE CurrentLifeStage { get; set; }

		public Insect(string sp, int legs)
		{
			this.Legs = legs;
			this.Species = sp;
			this.CurrentLifeStage = LIFESTAGE.EGG;
		}
		public virtual LIFESTAGE MoveLifeCycleForwards()
		{

			LIFESTAGE next = LIFESTAGE.ADULT;
			if (this.CurrentLifeStage == LIFESTAGE.EGG)
			{
				next = LIFESTAGE.NYMPH;
			}
			else if (this.CurrentLifeStage == LIFESTAGE.LARVA)
			{
				next = LIFESTAGE.PUPA;
			}
			this.CurrentLifeStage = next;
			return next;
		}
	}

	public class Fly : Insect
	{
		public override bool CanFly
		{
			get
			{
				return CurrentLifeStage == LIFESTAGE.ADULT;
			}
		}
		public override LIFESTAGE MoveLifeCycleForwards(){
			if(this.CurrentLifeStage==LIFESTAGE.EGG){
				LIFESTAGE next=LIFESTAGE.LARVA;
				this.CurrentLifeStage=next;
				return next;
			}
			else{
				return base.MoveLifeCycleForwards();
			}
		}
		public Fly(string sp, int legs) : base(sp, legs)
		{

		}
	}
}
