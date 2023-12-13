using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(UserSpecificationParams userParams)
            :base(x => 
                (string.IsNullOrEmpty(userParams.Search) || x.Name.Contains(userParams.Search)) &&
                (string.IsNullOrEmpty(userParams.Name) || x.Name.Contains(userParams.Name)) &&
                (string.IsNullOrEmpty(userParams.Lastname) || x.Lastname.Contains(userParams.Lastname))
            )
        {
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1),
                userParams.PageSize);

            if (!string.IsNullOrEmpty(userParams.Sort))
            {
                switch (userParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(u => u.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(u => u.Name);
                        break;
                    case "emailAsc":
                        AddOrderBy(u => u.Email);
                        break;
                    case "emailDesc":
                        AddOrderByDescending(u => u.Email);
                        break;
                    default:
                        AddOrderBy(u => u.Name);
                        break;
                }
            }
        }
    }
}
