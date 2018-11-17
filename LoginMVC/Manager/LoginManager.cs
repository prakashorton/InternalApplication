using LoginMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LoginMVC.Manager
{
    public class LoginManager
    {
        string connstring = null;
        SqlConnection conn = null;

        public LoginManager()
        {
            connstring = @"Data Source=LAPTOP-CS7AIJH9\SQLEXPRESS;Initial Catalog=DataTracks;Integrated Security=True";
            conn = new SqlConnection(connstring);
            conn.Open();
        }

        public Employee GetCurrentEmployee(string userName)
        {
            Employee employee = new Employee();
            string cmd = "SELECT * FROM [Employee] WHERE [UserName] = @UserName";
            using (SqlCommand loginCmd = new SqlCommand(cmd, this.conn))
            {
                loginCmd.Parameters.AddWithValue("@UserName", userName);
                SqlDataReader reader = loginCmd.ExecuteReader();
                reader.Read();
                employee.Id = (int)reader["Id"];
                employee.Name = (string)reader["Name"];
                employee.UserName = (string)reader["UserName"];
                employee.EmailId = (string)reader["EmailId"];
                employee.Role = (Role)reader["Role"];
                employee.Password = (string)reader["Password"];
                employee.IsActive = (bool)reader["IsActive"];
                reader.Close();
            }
            return employee;
        }

        public bool DeleteApplication(string id)
        {
            string cmd = @"DELETE FROM [Application] WHERE Id = " + id;
            using (SqlCommand command = new SqlCommand(cmd, this.conn))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }

        public Employee CheckLogin(Employee emp)
        {
            string cmd = "SELECT COUNT(*) FROM [Employee] WHERE [UserName] = @UserName and [Password] = @Password";
            using (SqlCommand loginCmd = new SqlCommand(cmd, this.conn))
            {
                loginCmd.Parameters.AddWithValue("@UserName", emp.UserName);
                loginCmd.Parameters.AddWithValue("@Password", emp.Password);
                int UserExist = (int)loginCmd.ExecuteScalar();
                if (!(UserExist > 0))
                {
                    emp = null;
                }
            }

            return emp;
        }

        public UIDropDown GetDropDownValues()
        {
            UIDropDown uIDropDown = new UIDropDown();
            string query = "select * FROM [ApplicationType] where IsActive = 1";
            using (SqlCommand applicationCmd = new SqlCommand(query, this.conn))
            {
                SqlDataReader reader = applicationCmd.ExecuteReader();
                // reader.Read();
                while (reader.Read())
                {
                    ApplicationType applicationType = new ApplicationType();
                    applicationType.Id = (int)reader["Id"];
                    applicationType.Name = (string)reader["Name"];
                    uIDropDown.ApplicationType.Add(applicationType);
                }
                reader.Close();
            }

            query = "select * FROM [Clients] where IsActive = 1";
            using (SqlCommand applicationCmd = new SqlCommand(query, this.conn))
            {
                SqlDataReader reader = applicationCmd.ExecuteReader();
                // reader.Read();
                while (reader.Read())
                {
                    Client client = new Client();
                    client.Id = (int)reader["Id"];
                    client.Name = (string)reader["Name"];
                    uIDropDown.Client.Add(client);
                }
                reader.Close();
            }

            query = "select * FROM [Environment] where IsActive = 1";
            using (SqlCommand applicationCmd = new SqlCommand(query, this.conn))
            {
                SqlDataReader reader = applicationCmd.ExecuteReader();
                // reader.Read();
                while (reader.Read())
                {
                    Models.Environment env = new Models.Environment();
                    env.Id = (int)reader["Id"];
                    env.Name = (string)reader["Name"];
                    uIDropDown.Environment.Add(env);
                }
                reader.Close();
            }

            query = "select * FROM [Server] where IsActive = 1";
            using (SqlCommand applicationCmd = new SqlCommand(query, this.conn))
            {
                SqlDataReader reader = applicationCmd.ExecuteReader();
                // reader.Read();
                while (reader.Read())
                {
                    Models.Server server = new Models.Server();
                    server.Id = (int)reader["Id"];
                    server.Name = (string)reader["Name"];
                    uIDropDown.Server.Add(server);
                }
                reader.Close();
            }

            query = "select * FROM [Technology] where IsActive = 1";
            using (SqlCommand applicationCmd = new SqlCommand(query, this.conn))
            {
                SqlDataReader reader = applicationCmd.ExecuteReader();
                // reader.Read();
                while (reader.Read())
                {
                    Models.Technology technology = new Models.Technology();
                    technology.Id = (int)reader["Id"];
                    technology.Name = (string)reader["Name"];
                    uIDropDown.Technology.Add(technology);
                }
                reader.Close();
            }
            return uIDropDown;
        }

        public Application AddApplication(Application application)
        {
            string cmdstr = @"INSERT INTO Application([Name],[Version],[PlannedRelease],[Description],[ActualRelease],
                            [TypeId], [EnvironmentId],[ServerId], [ClientId], [TechnologyId],[IsActive],[CreatedBy],
                            [CreatedDate], [ModifiedBy], [ModifiedDate]) 
                        output INSERTED.ID VALUES(@Name,@Version,@PlannedRelease,@Description,@ActualRelease,
                            @TypeId, @EnvironmentId,@ServerId, @ClientId, @TechnologyId,@IsActive,@CreatedBy,
                            @CreatedDate, @ModifiedBy, @ModifiedDate)";
            using (SqlCommand cmd = new SqlCommand(cmdstr, this.conn))
            {
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";

                cmd.Parameters.AddWithValue("@Name", application.Name);
                cmd.Parameters.AddWithValue("@Description", application.Description);
                cmd.Parameters.AddWithValue("@Version", application.Version);
                cmd.Parameters.AddWithValue("@PlannedRelease", application.PlannedRelease);
                cmd.Parameters.AddWithValue("@ActualRelease", application.ActualRelease);
                cmd.Parameters.AddWithValue("@EnvironmentId", Convert.ToInt32(application.Environment));
                cmd.Parameters.AddWithValue("@ClientId", Convert.ToInt32(application.Client));
                cmd.Parameters.AddWithValue("@ServerId", Convert.ToInt32(application.Server));
                cmd.Parameters.AddWithValue("@TechnologyId", Convert.ToInt32(application.Technology));
                cmd.Parameters.AddWithValue("@TypeId", Convert.ToInt32(application.Type));
                cmd.Parameters.AddWithValue("@IsActive", 1);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(application.CreatedBy));
                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToInt32(application.ModifiedBy));
                cmd.Parameters.AddWithValue("@ModifiedDate", time.ToString(format));
                cmd.Parameters.AddWithValue("@CreatedDate", time.ToString(format));
                application.Id = (int)cmd.ExecuteScalar();
            }

            return application;
        }

        public Employee RegisterUser(Employee register)
        {
            if (!ValidateUserName(register))
            {
                string cmdstr = @"INSERT INTO Employee(Name,UserName, Password, EmailId, Role,IsActive) 
                        output INSERTED.ID VALUES(@Name,@UserName, @Password, @EmailId, @Role, @IsActive)";
                using (SqlCommand cmd = new SqlCommand(cmdstr, this.conn))
                {
                    cmd.Parameters.AddWithValue("@Name", register.Name);
                    cmd.Parameters.AddWithValue("@UserName", register.UserName);
                    cmd.Parameters.AddWithValue("@Password", register.Password);
                    cmd.Parameters.AddWithValue("@EmailId", register.EmailId);
                    cmd.Parameters.AddWithValue("@Role", register.Role);
                    cmd.Parameters.AddWithValue("@IsActive", register.IsActive);
                    register.Id = (int)cmd.ExecuteScalar();
                }
            }
            else
            {
                throw new Exception(register.UserName + " ..!!! User name already exists");
            }
            return register;
        }

        public List<Application> GetAllApplication()
        {
            List<Application> applications = new List<Application>();
            string query = @"select app.Id,app.Name,
                app.Version,
                app.Description,app.PlannedRelease,
                app.ActualRelease,
                app.CreatedDate,
                appEnvironment.Name as EnvironmentName,
                apptype.Name as ApplicationType,
                appServer.Name as ServerName,
                emp.Name as CreatedBy,
                appServer.Provider,
                appClient.Name as ApplicationClientName,
                apptechnology.Name as ApplicationTechnology from [Application] as app
                join [ApplicationType] as apptype on app.TypeId = apptype.Id
                join [Technology] as apptechnology on app.TechnologyId = apptechnology.Id
                join [Environment] as appEnvironment on app.EnvironmentId = appEnvironment.Id
                join [Server] as appServer on app.ServerId = appServer.Id
                join [Clients] as appClient on app.ClientId = appClient.Id
                join [Employee] as emp on app.CreatedBy = emp.Id";
            using (SqlCommand applicationCmd = new SqlCommand(query, this.conn))
            {
                SqlDataReader reader = applicationCmd.ExecuteReader();
                // reader.Read();
                while (reader.Read())
                {
                    Application application = new Application();
                    application.Id = (int)reader["Id"];
                    application.Name = (string)reader["Name"];
                    application.Version = (string)reader["Version"];
                    application.PlannedRelease = (DateTime)reader["PlannedRelease"];
                    application.ActualRelease = (DateTime)reader["ActualRelease"];
                    application.Description = (string)reader["Description"];
                    application.CreatedBy = (string)reader["CreatedBy"];
                    application.CreatedDate = (DateTime)reader["CreatedDate"];
                    application.Environment = (string)reader["EnvironmentName"];
                    application.Client = (string)reader["ApplicationClientName"];
                    application.Type = (string)reader["ApplicationType"];
                    application.Server = (string)reader["ServerName"];
                    application.Provider = (string)reader["Provider"];
                    application.Technology = (string)reader["ApplicationTechnology"];
                    applications.Add(application);
                }
                reader.Close();
            }
            return applications;
        }

        private bool ValidateUserName(Employee employee)
        {
            bool isUsernameAvaiable = false;
            using (SqlCommand chkUserNameExists = new SqlCommand("SELECT COUNT(*) FROM [Employee] WHERE ([UserName] = @UserName)", this.conn))
            {
                chkUserNameExists.Parameters.AddWithValue("@UserName", employee.UserName);
                int UserExist = (int)chkUserNameExists.ExecuteScalar();
                if (UserExist > 0)
                {
                    isUsernameAvaiable = true;
                }
                return isUsernameAvaiable;
            }
        }
    }
}
