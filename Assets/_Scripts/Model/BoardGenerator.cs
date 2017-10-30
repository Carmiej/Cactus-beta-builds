using System;
using System.Xml;
using UnityEngine;
using Random = System.Random;

public class BoardGenerator : MonoBehaviour
{
	public int rows;
	public int columns;
	Room[,] rooms;
    Random rng;
    XmlElement root;

	public BoardGenerator ()
	{
        rng = new Random();
        XmlDocument roomFile = new XmlDocument();
        //roomFile.Load("Rooms.xml");
		roomFile.LoadXml(getXML());
        root = roomFile.DocumentElement;
        // DO NOT SET ROWS/COLUMNS LESS THAN 2
        rows = 9;
		columns = 9;
        int startRow = rows / 2;
        int startColumn = columns / 2;
        rooms = new Room[rows, columns];

		//XmlElement MAPElement = roomFile.GetElementById("MAP");
        Debug.Log("Root element name is " + root.Name);

        int[,] mock = new int[rows, columns];
        // initialize mock array to hold all -1
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                mock[i, j] = -1;
            }
        }

        // set starting room in mock array
        mock[startRow, startColumn] = 0;
        // set boss room above start room
        mock[startRow - 1, startColumn] = 1;

        addSurroundingRooms(startRow, startColumn, mock);

		string blah = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                blah += mock[i, j];
            }
			blah += '\n';
        }
		Debug.Log (blah);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                XmlNode thisRoom = root.SelectNodes("/MAP/ROOM[@ID='" + mock[i, j] + "']").Item(0);
                if (mock[i, j] == -1) // not part of the map
                {
                    rooms[i, j] = new Room(thisRoom, false, false, false, false);
                }

                bool n = false;
                if (i > 0 && i - 1 >= 0)
                {
                    n = true;
                }

                bool s = false;
                if (i < rows - 1 && i + 1 >= 0)
                {
                    s = true;
                }

                bool e = false;
                if (j < columns - 1 && j + 1 >= 0)
                {
                    e = true;
                }

                bool w = false;
                if (j > 0 && j - 1 >= 0)
                {
                    w = true;
                }

                rooms[i, j] = new Room(thisRoom, n, s, e, w);
                rooms[i, j].Start();
            }
        }
	}

    private void addSurroundingRooms (int roomRow, int roomColumn, int[,] mock)
    {
        
        // adding rooms to the left
        if (roomColumn > 0 && rng.Next(2) == 1 && mock[roomRow, roomColumn - 1] == -1)
        {
            
            mock[roomRow, roomColumn - 1] = rng.Next(2, Int32.Parse(root.GetAttribute("NUMROOMS")));
            addSurroundingRooms(roomRow, roomColumn - 1, mock);
        }
        // adding rooms to the right
        if (roomColumn < (columns - 1) && rng.Next(2) == 1 && mock[roomRow, roomColumn + 1] == -1)
        {
            mock[roomRow, roomColumn + 1] = rng.Next(2, Int32.Parse(root.GetAttribute("NUMROOMS")));
            addSurroundingRooms(roomRow, roomColumn + 1, mock);
        }
        // adding rooms above
        if (roomRow > 0 && rng.Next(2) == 1 && mock[roomRow - 1, roomColumn] == -1)
        {
            mock[roomRow - 1, roomColumn] = rng.Next(2, Int32.Parse(root.GetAttribute("NUMROOMS")));
            addSurroundingRooms(roomRow - 1, roomColumn, mock);
        }
        // adding rooms below
        if (roomRow < (rows - 1) && rng.Next(2) == 1 && mock[roomRow + 1, roomColumn] == -1)
        {
            mock[roomRow + 1, roomColumn] = rng.Next(2, Int32.Parse(root.GetAttribute("NUMROOMS")));
            addSurroundingRooms(roomRow + 1, roomColumn, mock);
        }
    }

    private string getXML ()
    {
        return (
            "<MAP NUMROOMS='3'>" +
            "<ROOM ID='-1' HEIGHT = '7' WIDTH = '9'>" +
                "<LAYOUT>" +
                    "NNNNNNNNN\n" +
                    "NNNNNNNNN\n" +
                    "NNNNNNNNN\n" +
                    "NNNNNNNNN\n" +
                    "NNNNNNNNN\n" +
                    "NNNNNNNNN\n" +
                    "NNNNNNNNN" +
                "</LAYOUT>" +
            "</ROOM>" +
            "<ROOM ID='0' HEIGHT = '7' WIDTH = '9'>" +
                "<LAYOUT>" +
                    "WWWWWWWWW\n" +
                    "WFFFFFFFW\n" +
                    "WFWFFFWFW\n" +
                    "WFFFFFFFW\n" +
                    "WFWFFFWFW\n" +
                    "WFFFFFFFW\n" +
                    "WWWWWWWWW" +
                "</LAYOUT>" +
            "</ROOM>" +
            "<ROOM ID='1' HEIGHT = '7' WIDTH = '9'>" +
                "<LAYOUT>" +
                    "WWWWWWWWW\n" +
                    "WFFFFFFFW\n" +
                    "WFFFFFFFW\n" +
                    "WFFFFFFFW\n" +
                    "WFFFFFFFW\n" +
                    "WFFFFFFFW\n" +
                    "WWWWWWWWW" +
                "</LAYOUT>\n" +
            "</ROOM>" +
            "<ROOM ID='2' HEIGHT = '7' WIDTH = '9'>" +
                "<LAYOUT>" +
                    "WWWWWWWWW\n" +
                    "WFFFFFFFW\n" +
                    "WFFFFFFFW\n" +
                    "WFFFWFFFW\n" +
                    "WFFFFFFFW\n" +
                    "WFFFFFFFW\n" +
                    "WWWWWWWWW" +
                "</LAYOUT>" +
            "</ROOM>" +
            "</MAP>"
            );
    }
}

