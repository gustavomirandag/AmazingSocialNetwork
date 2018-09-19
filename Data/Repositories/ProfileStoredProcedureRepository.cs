using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProfileStoredProcedureRepository : IProfileRepository
    {

        public void Create(Profile profile)
        {
            SqlConnection sqlConnection;
            sqlConnection = new SqlConnection("Server=tcp:myinfnetsocialnetwork.database.windows.net,1433;Initial Catalog=MySocialNetwork;Persist Security Info=False;User ID=olivato;Password=EDSInf123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            sqlConnection.Open();

            //######### INSERE NOVO PROFILE ##########
            SqlCommand sqlCommandAddProfile;
            sqlCommandAddProfile = new SqlCommand("AddProfile", sqlConnection);
            sqlCommandAddProfile.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommandAddProfile.Parameters.AddWithValue("Id", profile.Id);
            sqlCommandAddProfile.Parameters.AddWithValue("Name", profile.Name);
            sqlCommandAddProfile.Parameters.AddWithValue("Birthday", profile.Birthday);
            sqlCommandAddProfile.Parameters.AddWithValue("Photo", profile.Photo);
            sqlCommandAddProfile.Parameters.AddWithValue("Email", profile.Email);
            sqlCommandAddProfile.ExecuteNonQuery();
            //########################################

            sqlConnection.Close();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Profile Get(Guid? id)
        {
            SqlConnection sqlConnection;
            sqlConnection = new SqlConnection("Server=tcp:myinfnetsocialnetwork.database.windows.net,1433;Initial Catalog=MySocialNetwork;Persist Security Info=False;User ID=olivato;Password=EDSInf123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            sqlConnection.Open();

            Profile profile = new Profile();
            //######### OBTEM TODOS OS PROFILES ##########
            SqlCommand sqlCommandGetProfile;
            sqlCommandGetProfile = new SqlCommand("GetProfile", sqlConnection);
            sqlCommandGetProfile.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommandGetProfile.Parameters.AddWithValue("Id", id.ToString());
            var reader = sqlCommandGetProfile.ExecuteReader();

            while (reader.Read())
            {
                profile.Id = Guid.Parse(reader["Id"].ToString());
                profile.Name = reader["Name"].ToString();
                profile.Birthday = DateTime.Parse(reader["Birthday"].ToString());
                profile.Photo = reader["Photo"].ToString();
                profile.Email = reader["Email"].ToString();
            }
            //############################################

            sqlConnection.Close();
            return profile;
        }

        public IEnumerable<Profile> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Profile entity)
        {
            throw new NotImplementedException();
        }
    }
}
