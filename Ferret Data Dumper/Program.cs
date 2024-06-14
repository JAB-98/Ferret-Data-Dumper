using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ferret_Data_Dumper
{
    internal class Program
    {
        const string ip = "10.14.1.5";
        const string username = "sa";
        const string password = "sa";
        static void Main(string[] args)
        {
            GetData("StaffEmployment");
        }

        static void GetData(string db)
        {
            SqlConnecter sql = new SqlConnecter(ip, username, password, db);
            object[] docID = sql.GetObject(@"SELECT dbo.tblDocument.DocID FROM dbo.tblDocument INNER JOIN dbo.tblAccount ON dbo.tblDocument.DocAccountID = dbo.tblAccount.AccountID LEFT OUTER JOIN dbo.tblDocument AS LinkDoc ON dbo.tblAccount.AccountID = LinkDoc.DocAccountID AND dbo.tblDocument.DocLinkDocID = LinkDoc.DocID where dbo.tblDocument.DocAvailable=1");
            foreach (object obj in docID)
            {
                try
                {
                    object[] dockinfo = sql.GetObject($@"SELECT dbo.tblAccount.AccountNumber, dbo.tblAccount.AccountName, convert(varchar, dbo.tblDocument.DocDate, 23) , COALESCE (LinkDoc.DocDirPath, dbo.tblDocument.DocDirPath) + dbo.GetFolderForAccount(COALESCE (LinkDoc.DocAccountID, dbo.tblDocument.DocAccountID)) + '\' + COALESCE (LinkDoc.DocBarCode, dbo.tblDocument.DocBarCode) + '.' + COALESCE (LinkDoc.DocFileExt, dbo.tblDocument.DocFileExt) AS FullFilePath, (COALESCE (LinkDoc.DocBarCode, dbo.tblDocument.DocBarCode) + '.' + COALESCE (LinkDoc.DocFileExt, dbo.tblDocument.DocFileExt)) as FileName FROM dbo.tblDocument INNER JOIN dbo.tblAccount ON dbo.tblDocument.DocAccountID = dbo.tblAccount.AccountID LEFT OUTER JOIN dbo.tblDocument AS LinkDoc ON dbo.tblAccount.AccountID = LinkDoc.DocAccountID AND dbo.tblDocument.DocLinkDocID = LinkDoc.DocID where dbo.tblDocument.DocID='{obj}'");
                    string Account = $"{dockinfo[0]} - {dockinfo[1]}";
                    string date = dockinfo[2].ToString();
                    string from = dockinfo[3].ToString().Replace(@"\\SQL01\FerretDocs\", @"Z:\");
                    string file = dockinfo[4].ToString();
                    string to = $"Y:\\{db}\\{Account}\\{date}\\{file}";
                    string[] topart = to.Split('\\');
                    string pluspart = "";
                    Console.WriteLine(to);

                    for (int i = 0; i < topart.Length - 1; i++)
                    {
                        pluspart += topart[i] + "\\";
                        if (!Directory.Exists(pluspart))
                            Directory.CreateDirectory(pluspart);
                    }
                    File.Copy(from, to, false);

                }
                catch (Exception ex)
                {

                   Console.WriteLine(ex.ToString());
                }

            }
        }
    }
}
