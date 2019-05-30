using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Syncfusion.EJ2.FileManager.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Syncfusion.EJ2.FileManager.Base.SQLFileProvider
{
    public class SQLFileProvider : SQLFileProviderBase
    {                
        string ConnectionString;
        string TableName;
        List<string> deleteFilesId = new List<string>();
        string RootId;
        SqlConnection con;
        string SQLConnectionName;
        IConfiguration configuration;


        public SQLFileProvider(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        public SqlConnection setSQLDBConnection()
        {
            return new SqlConnection(@"" + this.ConnectionString);
        }

        public string ToCamelCase(FileManagerResponse userData)
        {
            return JsonConvert.SerializeObject(userData, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
        }

        public void SetSQLConnection(string name, string tableName, string tableID)
        {
            this.ConnectionString = this.configuration.GetConnectionString(name);
            this.TableName = tableName;
            this.RootId = tableID;
        }

        public FileManagerResponse GetFiles(string path, bool showHiddenItems, params FileManagerDirectoryContent[] data)
        {
            con = setSQLDBConnection();
            string ParentID = "";
            string IsRoot = "";
            if (path == "/")
            {
                ParentID = this.RootId;
                try
                {
                    con.Open();
                    string parentIDQuery = "select ItemID from " + this.TableName + " where ParentID='" + RootId + "'";
                    SqlCommand cmdd = new SqlCommand(parentIDQuery, con);
                    SqlDataReader reader = cmdd.ExecuteReader();
                    while (reader.Read())
                    {
                        IsRoot = reader["ItemID"].ToString();
                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    con.Close();

                }

            }
            else
            {
                try
                {
                    con.Open();
                    string parentIDQuery = "select ParentID from " + this.TableName + " where ItemID='" + data[0].Id + "'";
                    SqlCommand cmdd = new SqlCommand(parentIDQuery, con);
                    SqlDataReader reader = cmdd.ExecuteReader();
                    while (reader.Read())
                    {
                        ParentID = reader["ParentID"].ToString();
                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    con.Close();

                }
            }

            FileManagerDirectoryContent cwd = new FileManagerDirectoryContent();
            List<FileManagerDirectoryContent> files = new List<FileManagerDirectoryContent>();

            string query =
           "select * from " + this.TableName + "";
            SqlCommand cmd = new SqlCommand(query, con);
            FileManagerResponse readResponse = new FileManagerResponse();
            try
            {

                SqlConnection con = new SqlConnection(this.ConnectionString);
                string querystring = "";
                if (data.Length == 0)
                {
                    querystring = "select * from " + this.TableName + " where ParentID='" + ParentID + "'";
                } else{
                    querystring = "select * from " + this.TableName + " where ItemID='" + data[0].Id + "'";
                }
                try{
                    con.Open();
                    SqlCommand cmdd = new SqlCommand(querystring, con);
                    SqlDataReader reader = cmdd.ExecuteReader();
                    while (reader.Read())
                    {
                        cwd = new FileManagerDirectoryContent
                        {
                            Name = reader["Name"].ToString().Trim(),
                            Size = (long)reader["Size"],
                            IsFile = (bool)reader["IsFile"],
                            DateModified = (DateTime)reader["DateModified"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            Type = "",
                            Id = reader["ItemID"].ToString(),
                            HasChild = (bool)reader["HasChild"]
                        };

                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    con.Close();

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
            string FilesQueryString;
            if (path == "/")
            {
                FilesQueryString = " select * from " + this.TableName + " where ParentID = '" + IsRoot + "'";
            }
            else
            {
                FilesQueryString = "select * from " + this.TableName + " where ParentID='" + data[0].Id + "'";
            }

            try
            {
                con.Open();
                SqlCommand cmdd = new SqlCommand(FilesQueryString, con);
                SqlDataReader reader = cmdd.ExecuteReader();
                while (reader.Read())
                {
                    var childFiles = new FileManagerDirectoryContent
                    {
                        Name = reader["Name"].ToString().Trim(),
                        Size = (long)reader["Size"],
                        IsFile = (bool)reader["IsFile"],
                        DateModified = (DateTime)reader["DateModified"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        HasChild = (bool)reader["HasChild"],
                        FilterPath = data.Length != 0 ? path : "/",
                        Type = "",
                        Id = reader["ItemID"].ToString()
                    };
                    files.Add(childFiles);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();

            }
            readResponse.Files = files;
            readResponse.CWD = cwd;
            return readResponse;
        }

        public FileManagerResponse CopyTo(string path, string targetPath, string[] names, string[] replacedItemNames, params object[] data)
        {
            throw new NotImplementedException();
        }

        public FileManagerResponse CreateFolder(string path, string name, params FileManagerDirectoryContent[] data)
        {
            FileManagerResponse createResponse = new FileManagerResponse();
            try
            {

                FileManagerDirectoryContent CreateData = new FileManagerDirectoryContent();

                con = setSQLDBConnection();
                try
                {
                    con.Open();
                    string updateQuery = "update " + this.TableName + " SET HasChild='True' where ItemID='" + data[0].Id + "'";
                    SqlCommand updatecommand = new SqlCommand(updateQuery, con);
                    updatecommand.ExecuteNonQuery();
                    con.Close();
                    con.Open();
                    string ParentID = null;
                    string parentIDQuery = "select ParentID from " + this.TableName + " where ItemID='" + data[0].Id + "'";
                    SqlCommand cmdd = new SqlCommand(parentIDQuery, con);
                    SqlDataReader RD = cmdd.ExecuteReader();
                    while (RD.Read())
                    {
                        ParentID = RD["ParentID"].ToString();
                    }
                    con.Close();
                    con.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO " + TableName + " (Name, ParentID, Size, IsFile, MimeType, DateModified, DateCreated, HasChild, IsRoot, Type) VALUES ( @Name, @ParentID, @Size, @IsFile, @MimeType, @DateModified, @DateCreated, @HasChild, @IsRoot, @Type )", con);
                    command.Parameters.Add(new SqlParameter("@Name", name.Trim()));
                    command.Parameters.Add(new SqlParameter("@ParentID", data[0].Id));
                    command.Parameters.Add(new SqlParameter("@Size", 30));
                    command.Parameters.Add(new SqlParameter("@IsFile", false));
                    command.Parameters.Add(new SqlParameter("@MimeType", "Folder"));
                    command.Parameters.Add(new SqlParameter("@DateModified", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@DateCreated", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@HasChild", false));
                    command.Parameters.Add(new SqlParameter("@IsRoot", false));
                    command.Parameters.Add(new SqlParameter("@Type", "Folder"));


                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        CreateData = new FileManagerDirectoryContent
                        {
                            Name = reader["Name"].ToString().Trim(),
                            Size = (long)reader["Size"],
                            IsFile = (bool)reader["IsFile"],
                            DateModified = (DateTime)reader["DateModified"],
                            DateCreated = (DateTime)reader["DateCreated"]
                            ,
                            Type = ""
                           ,
                            HasChild = (bool)reader["HasChild"]
                        };

                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    con.Close();

                }


                var newData = new FileManagerDirectoryContent[] { CreateData };
                createResponse.Files = newData;
                return createResponse;
            }
            catch (Exception e)
            {
                ErrorDetails er = new ErrorDetails();
                er.Code = "404";
                er.Message = e.Message.ToString();
                createResponse.Error = er;

                return createResponse;
            }
        }
        private FileStreamResult fileStreamResult;
        public FileStreamResult Download(string path, string[] names, params FileManagerDirectoryContent[] data)
        {
            if (data != null)
            {
                byte[] fileContent;
                con = setSQLDBConnection();
                foreach (FileManagerDirectoryContent item in data)
                {
                    con.Open();
                    SqlCommand myCommand = new SqlCommand("select * from " + TableName + " where ItemId =" + item.Id, con);
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        fileContent = (byte[])myReader["Content"];
                        if (File.Exists(Path.Combine(Path.GetTempPath(), names[0])))
                        {
                            File.Delete(Path.Combine(Path.GetTempPath(), names[0]));
                        }
                        using (Stream file = File.OpenWrite(Path.Combine(Path.GetTempPath(), names[0])))
                        {
                            file.Write(fileContent, 0, fileContent.Length);
                        }
                        try
                        {
                            FileStream fileStreamInput = new FileStream(Path.Combine(Path.GetTempPath(), names[0]), FileMode.Open, FileAccess.Read);
                            fileStreamResult = new FileStreamResult(fileStreamInput, "APPLICATION/octet-stream");
                            fileStreamResult.FileDownloadName = data[0].Name;
                        }
                        catch (Exception ex) { throw ex; }
                    }
                    con.Close();
                }
            }
            return fileStreamResult;
        }


        public FileManagerResponse GetDetails(string path, string[] names, params FileManagerDirectoryContent[] data)
        {
            con = setSQLDBConnection();
            FileManagerResponse getDetailResponse = new FileManagerResponse();
            FileDetails detailFiles = new FileDetails();
            string querystring;
            if (data[0].Id == null)
            {
                querystring = "select * from " + this.TableName + " where ItemID='" + this.RootId + "'";
            }
            else
            {
                querystring = "select * from " + this.TableName + " where ItemID='" + data[0].Id + "'";
            }

            try
            {
                con.Open();
                SqlCommand cmdd = new SqlCommand(querystring, con);
                SqlDataReader reader = cmdd.ExecuteReader();
                while (reader.Read())
                {
                    if (names.Length == 1)
                    {
                        detailFiles = new FileDetails
                        {
                            Name = reader["Name"].ToString().Trim(),
                            Size = reader["Size"].ToString(),
                            IsFile = (bool)reader["IsFile"],
                            Modified = (DateTime)reader["DateModified"],
                            Created = (DateTime)reader["DateCreated"],
                            Location = path + names[0]
                        };
                    }
                    else
                    {
                        detailFiles = new FileDetails
                        {
                            Name = string.Join(", ", names),
                            Size = reader["Size"].ToString(),
                            MultipleFiles = true,
                            Location = path
                        };
                    }

                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();

            }

            getDetailResponse.Details = detailFiles;
            return getDetailResponse;
        }

        public FileStreamResult GetImage(string path, bool allowCompress, params object[] data)
        {
            throw new NotImplementedException();
        }

        public FileManagerResponse MoveTo(string path, string targetPath, string[] names, string[] replacedItemNames, params object[] data)
        {
            throw new NotImplementedException();
        }

        public FileManagerResponse Remove(string path, string[] names, params FileManagerDirectoryContent[] data)
        {
            FileManagerResponse remvoeResponse = new FileManagerResponse();
            string ParentID = "";
            try
            {
                FileManagerDirectoryContent DeletedData = new FileManagerDirectoryContent();
                List<FileManagerDirectoryContent> newData = new List<FileManagerDirectoryContent>();
                List<string> deleteSubs = new List<string>();
                con = setSQLDBConnection();
                foreach (var file in data)
                {
                    try
                    {
                        con.Open();
                        string parentIDQuery = "select ParentID from " + this.TableName + " where ItemID='" + file.Id + "'";
                        SqlCommand cmdd = new SqlCommand(parentIDQuery, con);
                        SqlDataReader reader = cmdd.ExecuteReader();
                        while (reader.Read())
                        {
                            ParentID = reader["ParentID"].ToString();
                        }

                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        con.Close();

                    }
                    try
                    {
                        Int32 count;
                        con.Open();
                        SqlCommand Checkcommand = new SqlCommand("select COUNT(*) from " + this.TableName + " where ParentID='" + ParentID + "' AND MimeType= 'folder' AND Name <> '" + file.Name + "'", con);
                        count = (Int32)Checkcommand.ExecuteScalar();
                        con.Close();
                        if (count == 0)
                        {
                            con.Open();
                            string updateQuery = "update " + this.TableName + " SET HasChild='False' where itemId='" + ParentID + "'";
                            SqlCommand updatecommand = new SqlCommand(updateQuery, con);
                            updatecommand.ExecuteNonQuery();
                            con.Close();
                        }
                        con.Open();
                        SqlCommand command = new SqlCommand("select * from " + this.TableName + " where ParentID='" + ParentID + "'", con);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            DeletedData = new FileManagerDirectoryContent
                            {
                                Name = reader["Name"].ToString().Trim(),
                                Size = (long)reader["Size"],
                                IsFile = (bool)reader["IsFile"],
                                DateModified = (DateTime)reader["DateModified"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                Type = "",
                                HasChild = (bool)reader["HasChild"],
                                Id = reader["ItemID"].ToString()
                            };
                            if (file.Name == DeletedData.Name) { deleteSubs.Add(DeletedData.Id); }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        con.Close();

                    }

                    try
                    {
                        con.Open();
                        SqlCommand DelCmd = new SqlCommand("delete  from " + this.TableName + " where ItemID='" + file.Id + "'", con);
                        SqlDataReader DelRd = DelCmd.ExecuteReader();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        con.Close();

                    }
                    newData.Add(DeletedData);
                    remvoeResponse.Files = newData;
                    updateTableOnDelete(deleteSubs.Distinct().ToArray());
                }
                return remvoeResponse;
            }
            catch (Exception e)
            {
                ErrorDetails er = new ErrorDetails();
                er.Code = "404";
                er.Message = e.Message.ToString();
                remvoeResponse.Error = er;

                return remvoeResponse;
            }
        }
        public void updateTableOnDelete(string[] ids)
        {
            con.Open();
            foreach (var id in ids)
            {
                if (this.deleteFilesId.IndexOf(id) == -1)
                {
                    this.deleteFilesId.Add(id);
                }
                SqlCommand deleteSubs = new SqlCommand("select * from " + this.TableName + " where ParentID='" + id + "'", con);
                SqlDataReader deleteSubsReader = deleteSubs.ExecuteReader();
                while (deleteSubsReader.Read())
                {
                    this.deleteFilesId.Add(deleteSubsReader["ItemID"].ToString());
                    if (!(bool)deleteSubsReader["IsFile"])
                    {
                        con.Close();
                        updateTableOnDelete(new[] { deleteSubsReader["ItemID"].ToString() });
                    }
                }
            }
            con.Close();
        }
        public virtual FileManagerResponse Upload(string path, IList<IFormFile> uploadFiles, string action, string[] replacedItemNames, params FileManagerDirectoryContent[] data)
        {
            FileManagerResponse uploadResponse = new FileManagerResponse();
            string filename = Path.GetFileName(uploadFiles[0].FileName);
            string contentType = uploadFiles[0].ContentType;
            try
            {

                using (FileStream fsSource = new FileStream(Path.Combine(Path.GetTempPath(), filename), FileMode.Create))
                {
                    uploadFiles[0].CopyTo(fsSource);
                    byte[] bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
                        if (n == 0)
                        {

                            break;
                        }

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = bytes.Length;
                    UploadQuery(filename, contentType, bytes, data[0].Id);

                }
            }
            catch (FileNotFoundException e)
            {

            }
            return uploadResponse;
        }

        public void UploadQuery(string filename, string contentType, byte[] bytes, string parentId)
        {
            con = setSQLDBConnection();
            con.Open();
            SqlCommand command = new SqlCommand("INSERT INTO " + TableName + " (Name, ParentID, Size, IsFile, MimeType, DateModified, DateCreated, HasChild, IsRoot, Type) VALUES ( @Name, @ParentID, @Size, @IsFile, @MimeType, @DateModified, @DateCreated, @HasChild, @IsRoot, @Type )", con);
            command.Parameters.Add(new SqlParameter("@Name", filename));
            command.Parameters.Add(new SqlParameter("@IsFile", true));
            command.Parameters.Add(new SqlParameter("@Size", 20));
            command.Parameters.Add(new SqlParameter("@ParentId", parentId));
            command.Parameters.Add(new SqlParameter("@MimeType", contentType));
            command.Parameters.Add(new SqlParameter("@Content", bytes));
            command.Parameters.Add(new SqlParameter("@DateModified", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@DateCreated", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@HasChild", false));
            command.Parameters.Add(new SqlParameter("@IsRoot", false));
            command.Parameters.Add(new SqlParameter("@Type", "File"));
            command.ExecuteNonQuery();
        }
        public byte[] FileToByteArray(string fileName)
        {
            byte[] fileData = null;

            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    fileData = binaryReader.ReadBytes((int)fs.Length);
                }
            }
            return fileData;
        }

        public FileManagerResponse Rename(string path, string name, string newName, bool replace = false, params FileManagerDirectoryContent[] data)
        {
            FileManagerResponse remvoeResponse = new FileManagerResponse();
            try
            {
                FileManagerDirectoryContent DeletedData = new FileManagerDirectoryContent();
                con = setSQLDBConnection();
                try
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(" update Product set Name='" + newName + "' where ItemID ='" + data[0].Id + "'", con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DeletedData = new FileManagerDirectoryContent
                        {
                            Name = reader["Name"].ToString().Trim(),
                            Size = (long)reader["Size"],
                            IsFile = (bool)reader["IsFile"],
                            DateModified = (DateTime)reader["DateModified"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            Type = "",
                            HasChild = (bool)reader["HasChild"]
                        };

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    con.Close();

                }
                var newData = new FileManagerDirectoryContent[] { DeletedData };
                remvoeResponse.Files = newData;
                return remvoeResponse;
            }
            catch (Exception e)
            {
                ErrorDetails er = new ErrorDetails();
                er.Code = "404";
                er.Message = e.Message.ToString();
                remvoeResponse.Error = er;

                return remvoeResponse;
            }
        }

        public FileManagerResponse Search(string path, string searchString, bool showHiddenItems, bool caseSensitive, params FileManagerDirectoryContent[] data)
        {
            con = setSQLDBConnection();
            FileManagerResponse searchResponse = new FileManagerResponse();
            try
            {
                if (path == null) { path = string.Empty; };

                var searchWord = searchString;
                FileManagerDirectoryContent searchData;
                FileManagerDirectoryContent cwd = new FileManagerDirectoryContent();
                cwd.Name = data[0].Name;
                cwd.Size = data[0].Size;
                cwd.IsFile = false;
                cwd.DateModified = data[0].DateModified;
                cwd.DateCreated = data[0].DateCreated;
                cwd.HasChild = data[0].HasChild;
                cwd.Type = data[0].Type;
                cwd.FilterPath = path + data[0].Name;
                searchResponse.CWD = cwd;
                List<FileManagerDirectoryContent> foundedFiles = new List<FileManagerDirectoryContent>();
                con.Open();
                SqlCommand searchCommand = new SqlCommand("select * from " + this.TableName + " where Name like '" + searchString.Replace("*", "%") + "'", con);
                SqlDataReader reader = searchCommand.ExecuteReader();
                while (reader.Read())
                {
                    searchData = new FileManagerDirectoryContent
                    {
                        Name = reader["Name"].ToString().Trim(),
                        Size = (long)reader["Size"],
                        IsFile = (bool)reader["IsFile"],
                        DateModified = (DateTime)reader["DateModified"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        Type = "",
                        HasChild = (bool)reader["HasChild"],
                        Id = reader["ItemId"].ToString().Trim()
                    };
                    if (searchData.Name != "Products") foundedFiles.Add(searchData);
                }
                searchResponse.Files = (IEnumerable<FileManagerDirectoryContent>)foundedFiles;
                return searchResponse;
            }
            catch (Exception e)
            {
                ErrorDetails er = new ErrorDetails();
                er.Code = "404";
                er.Message = e.Message.ToString();
                searchResponse.Error = er;

                return searchResponse;
            }
            finally
            {
                con.Close();
            }
        }

        public FileManagerResponse Upload(string path, IList<IFormFile> uploadFiles, string action, string[] replacedItemNames, params object[] data)
        {
            throw new NotImplementedException();
        }


        public FileManagerResponse CopyTo(string path, string targetPath, string[] names, string[] replacedItemNames, params FileManagerDirectoryContent[] data)
        {
            throw new NotImplementedException();
        }

        public FileManagerResponse MoveTo(string path, string targetPath, string[] names, string[] replacedItemNames, params FileManagerDirectoryContent[] data)
        {
            throw new NotImplementedException();
        }

        public FileStreamResult GetImage(string path, bool allowCompress, ImageSize size, params FileManagerDirectoryContent[] data)
        {
            throw new NotImplementedException();
        }
    }
}

