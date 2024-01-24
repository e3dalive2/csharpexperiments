using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        public class NotesStore
        {
            Dictionary<String, List<String>> notes = new Dictionary<String, List<String>>();



            public NotesStore() { }
            public void AddNote(String state, String name)
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new Exception("Name cannot be empty");
                }

                if (!isGoodState(state))
                {
                    throw new Exception("Invalid state " + state);
                }

                List<String> val;
                if (notes.TryGetValue(state, out val))
                {

                }
                else
                {
                    val = new List<String>();
                    notes[state] = val;
                }

                val.Add(name);
            }

            bool isGoodState(String state)
            {
                return state == "completed" || state == "active" || state == "others";
            }

            public List<String> GetNotes(String state)
            {
                if (!isGoodState(state))
                {
                    throw new Exception("Invalid state " + state);
                }

                List<String> val;
                if (notes.TryGetValue(state, out val))
                {
                    return val;
                }
                return new List<String>();
            }
        }


        static void Main(string[] args)
        {
            NotesStore store = new NotesStore();
            store.AddNote("active", "DrinkTea");
            store.AddNote("active", "DrinkCoffee");
            store.AddNote("completed", "Study");
            store.GetNotes("active");
            store.GetNotes("completed");
            store.GetNotes("foo");
        }
    }
}