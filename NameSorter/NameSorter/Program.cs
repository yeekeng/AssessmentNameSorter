using NameSorter.Common;
using NameSorter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NameSorter
{
    public class Program
    {
        private static readonly string unsortedNamesFile = @"..\..\..\Documents\unsorted-names-list.txt";
        private static readonly string sortedNamesFile = @"..\..\..\Documents\sorted-names-list.txt";

        private static void Main(string[] args)
        {
            try
            {
                //check if file exist
                if (File.Exists(unsortedNamesFile))
                {
                    //get list of names
                    List<string> unsortedNameList = GetListOfNameFromFile(unsortedNamesFile);

                    if (unsortedNameList.Any())
                    {
                        //create person list from list of names
                        List<Person> unsortedPersonList = CreatePersonList(unsortedNameList);

                        //sort person list
                        List<Person> sortedPersonList = SortPersonList(unsortedPersonList);

                        //get names to print in string
                        string namesToPrint = GetNameListForPrinting(sortedPersonList);

                        //write to file
                        File.WriteAllText(sortedNamesFile, namesToPrint);

                        //print out
                        Console.WriteLine(namesToPrint);
                    }
                }
                else
                {
                    //no file
                    Console.WriteLine("File not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                //ideally there are some logging function here that prints out ex or store the exception somewhere
            }
        }

        /// <summary>
        /// Read the file and return a list of names.
        /// each line is trim to prevent bad spacing of names before and after.
        /// blank lines are ignored.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private static List<string> GetListOfNameFromFile(string FilePath)
        {
            List<string> nameList = new List<string>();
            try
            {
                using (StreamReader file = new StreamReader(FilePath))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            nameList.Add(line.Trim());
                        }
                    }
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                // logger
                // throw
            }

            return nameList;
        }

        /// <summary>
        /// create a list of Person from a list of names.
        /// empty lines or names with wrong spacing will be ignore
        /// return the personList even if there are exception
        /// </summary>
        /// <param name="unsortedNameList"></param>
        /// <returns></returns>
        private static List<Person> CreatePersonList(IEnumerable<string> nameList)
        {
            List<Person> personList = new List<Person>();

            try
            {
                foreach (string name in nameList)
                {
                    //get index of last space
                    int lastSpaceIndex = name.LastIndexOf(Separator.SPACE);

                    if (lastSpaceIndex > 0)
                    {
                        string lastName = name.Substring(lastSpaceIndex + 1);
                        string givenName = name.Remove(lastSpaceIndex);

                        //create the person
                        Person p = new Person();
                        p.LastName = lastName;
                        p.GivenName = givenName;

                        //add to list
                        personList.Add(p);
                    }
                    else if (lastSpaceIndex == -1)
                    {
                        // if only 1 given name
                        //create the person
                        Person p = new Person();
                        p.LastName = null;
                        p.GivenName = name;

                        //add to list
                        personList.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                //logger here
                //throw if needed
            }

            return personList;
        }

        /// <summary>
        /// sort the list by last name then by given name
        /// </summary>
        /// <param name="unsortedPersonList"></param>
        /// <returns></returns>
        private static List<Person> SortPersonList(List<Person> unsortedPersonList)
        {
            List<Person> list = null;
            try
            {
                list = unsortedPersonList.OrderBy(a => a.LastName)
                                        .ThenBy(a => a.GivenName)
                                        .ToList();
            }
            catch (Exception ex)
            {
                //logger and/or throw
            }

            return list;
        }

        /// <summary>
        /// convert the list of sorted names to string for printing
        /// </summary>
        /// <param name="sortedPersonList"></param>
        /// <returns></returns>
        private static string GetNameListForPrinting(List<Person> nameList)
        {
            StringBuilder nameListBuilder = new StringBuilder();

            try
            {
                if (nameList.Any())
                {
                    foreach (Person p in nameList)
                    {
                        nameListBuilder.AppendLine(p.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //logger and/or throw
            }

            return nameListBuilder.ToString();
        }
    }
}