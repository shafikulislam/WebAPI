using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.ViewModel;

namespace WebAPI.Models.Entity
{
	public class UserInfo
	{
		[Key]
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public bool IsDeleted { get; set; }

		//[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		//[DefaultValue("getutcdate()")]
		public DateTime CreatedAt { get; set; }
		public UserInfo DeletedBy { get; set; }
		public DateTime DeletedAt { get; set; }

	}
}
