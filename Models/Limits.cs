using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;

namespace Asp_Web_Lib.Models
{
	public class Limits
	{
		//id dla zmian limitów
		[Key]
		public int IdLimits { get; set; }

		[Required]
		//Liczba wypożyczonych książek w jednym momencie
		public int MaxBorrowedBooks {  get; set; }

		[Required]
		//Liczba ksiązek w kolejce
		public int MaxWaitingBooks	{ get; set; }

		[Required]
		//Liczba możliwych przedłużeń
		public int MaxExtensionNumber { get; set; }

		[Required]
		//Liczba dni w przedłużeniu/wypożyczeniu
		public int ExtensionDays { get; set; }

		[Required]
		//Kwota za dzień opóźnienia
		public decimal LoanAmount { get; set; }
	}
}