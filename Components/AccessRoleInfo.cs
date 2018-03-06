/*
' Copyright (c) 2017 Don Hoang
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Web.Caching;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;

namespace DBH.ApplicationRole.Components
{

    public class AccessRoleInfo
    {
        public string Username { get; set; }
        public int UserID { get; set; }
        public int UserRoleID { get; set; }
        public int AffiliateID { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Email { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsOwner { get; set; }
        public bool IsTrialUsed { get; set; }
        public bool Subscribed { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSuperUser { get; set; }
        public string LastIPAddress { get; set; }
        public string LastName { get; set; }
        public DateTime PasswordResetExpiration { get; set; }
        public Guid PasswordResetToken { get; set; }
        public int PortalID { get; set; }
        public bool RefreshRoles { get; set; }
        public string[] Roles { get; set; }
    }

}
