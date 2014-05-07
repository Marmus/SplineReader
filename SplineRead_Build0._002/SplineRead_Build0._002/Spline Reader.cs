using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

//
//Team Fusion Map Making Department
//by ATAG_Marmus 
//
//Build v0.5 May 5, 2014
//----Rev v0.5.1 May 6, 2014 - fixed clear textbox screen, added static parse of 0 or 1 DefPoint value to parse roads from outlines, other misc changes
//
namespace SplineRead_Build0._002
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OpenFileDialog ofd = new OpenFileDialog();

        public void button1_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            //BinaryReader br = new BinaryReader(File.OpenRead(ofd.FileName));
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            FileStream headerin = new FileStream((ofd.FileName), FileMode.Open,
            FileAccess.Read, FileShare.ReadWrite);
            //Create a BinaryReader from the FileStream
            BinaryReader br = new BinaryReader(headerin);
            //Seek to the start of the file
            br.BaseStream.Seek(0, SeekOrigin.Begin); //the 0 is the offset from start of file
            //Read the Header Data from the file and store the values to the variables
            int Header = br.ReadInt32();
            int UnknownHdr01 = br.ReadInt32();
            int UnknownHdr02 = br.ReadInt32();
            int UnknownHdr03 = br.ReadInt32();
            int UnknownHdr04 = br.ReadInt32();
            int UnknownHdr05 = br.ReadInt32();
            int NumSplines = br.ReadInt32();
            //Display the data on the console            
            textBox1.AppendText("File Header: \t\t" + Header.ToString() + "\r\n");
            textBox1.AppendText("Unknown Header 01: \t" + UnknownHdr01.ToString() + "\r\n");            
            textBox1.AppendText("Unknown Header 02 :\t" + UnknownHdr02 + "\r\n");
            textBox1.AppendText("Unknown Header 03 :\t" + UnknownHdr03 + "\r\n");
            textBox1.AppendText("Unknown Header 04 :\t" + UnknownHdr04 + "\r\n");
            textBox1.AppendText("Unknown Header 05 :\t" + UnknownHdr05 + "\r\n");
            textBox1.AppendText("Number of Splines in File :\t" + NumSplines + "\r\n");
                        //Close the stream and free the resources
            br.Close(); //closes the instance that read file Header only
            textBox1.AppendText("File Header Read!\r\n");
            textBox1.AppendText("====================================================== \r\n");

            //progress bar setup
            progressBar2.Minimum = 1;
            progressBar2.Maximum = NumSplines;


            /*
             * spline FMB format
            [splines]
                3257 0_SplineRoad 13/width 5
            [0_SplineRoad]
                311608.1 16291.853 20.00 null R 311608.1 16388.785 20.00 -1
                311582.676 16681.414 20.00  L 311601.211 16887.653 20.00  R 311536.493 17160.57201 20.00 -1
                311401.833 17691.95903 20.00  L 311268.878 18136.71803 20.00  R 311134.203 18571.91804 20.00 -1
                311024.48 19347.84004 20.00  L 310888.819 20098.79207 20.00 R 311024.48 19347.84004 20.00 -1
                310888.819 20098.79207 20.00  L 310803.095 20800.88509 20.00 R 310838.407 21646.0931 20.00 -1
                310864.338 22439.09511 20.00  L 310909.206 23240.55013 20.00 R 310975.594 24246.51213 20.00 -1
                310989.979 25237.81212 20.00  L 311068.544 26150.27513 20.00 R 311278.338 27189.34613 20.00 -1
                311458.216 28190.15513 20.00  L 311697.355 29169.82414 20.00 R 311977.463 30334.98014 20.00 -1
                312247.779 31453.51314 20.00  L 312533.61 32548.98814 20.00 R 312846.602 33791.85214 20.00 -1
                313131.749 35034.70014 20.00  L 313465.864 36180.64504 20.00 null -1
            */

            FileStream fin = new FileStream((ofd.FileName), FileMode.Open,
            FileAccess.Read, FileShare.ReadWrite); //open file again to do loops
            //Create a BinaryReader from the FileStream
            BinaryReader br2 = new BinaryReader(fin);
            //Seek to the start of the file
            //attempt to add a loop
            int counter = 5000;  //         <<<------changing to get smaller chunks of data out original line is: int counter = NumSplines - 1;
            int pos = 28; //position of first spline
            
            textBox2.AppendText("[splines]\r\n");
            for (int i = 0; i <= counter; i++)
            {
                //textBox1.AppendText("File Position" + pos + "\r\n");
                br2.BaseStream.Seek(pos, SeekOrigin.Begin);
                int DefPoints = br2.ReadInt32();
                int UnknownData01 = br2.ReadInt32();
                int UnknownData02 = br2.ReadInt32();
                uint IDNumber = br2.ReadUInt32();
                int UnknownData03 = br2.ReadInt32();
                int unknownData04 = br2.ReadInt32();
                int UnknownData05 = br2.ReadInt32();
                int splineLength = br2.ReadInt32();
                //start double
                double Xp_Origin = br2.ReadDouble();
                double Yp_Origin = br2.ReadDouble();
                double xL_Origin = br2.ReadDouble();
                double yL_Origin = br2.ReadDouble();
                double xR_Origin = br2.ReadDouble();
                double yR_Origin = br2.ReadDouble();
                //float Xpn = br2.ReadSingle();
                //int pass = i;
                //textBox2.Text = "Pass" + i;
                
                if (DefPoints == 1) //0 for Roads/Tracks/Rivers   1 for layout outlines   <<<<<-------change value to print out roads/trains or layout outlines
                {


                
                progressBar2.PerformStep();

                textBox1.AppendText("File Position" + pos + "\r\n");
                //Display the data on the textBox1  
                textBox1.AppendText("     \r\n");
                textBox1.AppendText("Spline \t\t\t" + "no." + (i + 1) + "\r\n");
                textBox1.AppendText("Spline Header Value :\t" + DefPoints + "\r\n");
                textBox1.AppendText("Unknown 01 :\t\t" + UnknownData01 + "\r\n");
                textBox1.AppendText("Unknown 02 :\t\t" + UnknownData02 + "\r\n");
                textBox1.AppendText("Spline ID Number :\t\t" + IDNumber + "\r\n");
                textBox1.AppendText("Unknown 03 :\t\t" + UnknownData03 + "\r\n");
                textBox1.AppendText("Unknown 04 :\t\t" + unknownData04 + "\r\n");
                textBox1.AppendText("Unknown 05 :\t\t" + UnknownData05 + "\r\n");
                textBox1.AppendText("Spline Length :\t\t" + splineLength + "\r\n");
                //start double
                // below used to list coordinates
                textBox1.AppendText("-------------------------------------------------------------------------------------------------------------------------------------------------------\r\n");
                textBox1.AppendText("     \r\n");
                /*
                textBox1.AppendText(" Origin Coordinates (Global)\r\n");
                textBox1.AppendText("Origin X-Coord :\t" + Xp_Origin + "\r\n");
                textBox1.AppendText("Origin Y-Coord :\t" + Yp_Origin + "\r\n");
                textBox1.AppendText("Origin xL-Coord :\t" + xL_Origin + "\r\n");
                textBox1.AppendText("Origin yL-Coord :\t" + yL_Origin + "\r\n");
                textBox1.AppendText("Origin xR-Coord :\t" + xR_Origin + "\r\n");
                textBox1.AppendText("Origin yR-Coord :\t" + yR_Origin + "\r\n");
                */
                //following is to get into FMB spline format
                //
                //textBox1.AppendText("\r\n");
               // textBox1.AppendText("Spline into FMB format\r\n");
                //textBox1.AppendText("-------------------------------------------------------------------------------------------------------------------------------------------------------\r\n");
                //textBox2.AppendText("[splines]\r\n");
                textBox2.AppendText(IDNumber + " " + i + "_SplineRoad 13/width 5\r\n");
                //textBox2.AppendText("\r\n");
                textBox3.AppendText("[" + i + "_SplineRoad]\r\n");
                textBox3.AppendText(Math.Round(Xp_Origin, 2) + " ");
                textBox3.AppendText(Math.Round(Yp_Origin, 2) + " 20.00 ");
                //next 2 lines commented out to replace value with NULL
                //textBox3.AppendText("L " + Math.Round(xL_Origin, 2) + " ");
                //textBox3.AppendText(Math.Round(yL_Origin, 2) + " 20.00 R ");
                textBox3.AppendText("null R ");
                textBox3.AppendText(Math.Round(xR_Origin, 2) + " ");
                textBox3.AppendText(Math.Round(yR_Origin, 2) + " 20.00 -1\r\n");
                


                
                //textBox1.AppendText("     \r\n");
                //textBox1.AppendText("Control Points (Local Coordinates) :\r\n");
                int jpos = (splineLength - 1);
                for (int j = 1; j <= jpos; j++)
                {
                    //textBox1.AppendText("     \r\n");
                    //textBox1.AppendText("Control Point " + (j + 1) + " :\r\n");

                    float XpnLocal = br2.ReadSingle();
                    double Xpn = Math.Round((Xp_Origin + XpnLocal), 2);
                    //textBox1.AppendText("Xp" + (j + 1) + " :" + XpnLocal + "\r\n");

                    float YpnLocal = br2.ReadSingle();
                    double Ypn = Math.Round((Yp_Origin + YpnLocal), 2);
                    //textBox1.AppendText("Yp" + (j + 1) + " :" + YpnLocal + "\r\n");

                    float XLnLocal = br2.ReadSingle();
                    double XLn = Math.Round((xL_Origin + XLnLocal), 2);
                    //textBox1.AppendText("xL" + (j + 1) + " :" + XLnLocal + "\r\n");

                    float YLnLocal = br2.ReadSingle();
                    double YLn = Math.Round((yL_Origin + YLnLocal), 2);
                    //textBox1.AppendText("yL" + (j + 1) + " :" + YLnLocal + "\r\n");

                    float XRnLocal = br2.ReadSingle();
                    double XRn = Math.Round((xR_Origin + XRnLocal), 2);
                    //textBox1.AppendText("xR" + (j + 1) + " :" + XRnLocal + "\r\n");

                    float YRnLocal = br2.ReadSingle();
                    double YRn = Math.Round((yR_Origin + YRnLocal), 2);
                    //textBox1.AppendText("yR" + (j + 1) + " :" + YRnLocal + "\r\n");
                    //
                    //below is to get all control points into FMB spline format
                    //
                    textBox3.AppendText(Xpn + " ");
                    textBox3.AppendText(Ypn + " 20.00 L ");
                    textBox3.AppendText(XLn + " ");
                    textBox3.AppendText(YLn + " 20.00 ");
                    //add if statement here to get NULL value at last string
                    if (j == jpos)
                    {
                        textBox3.AppendText("null -1\r\n");
                    }
                    else
                    {
                        textBox3.AppendText("R " + XRn + " ");
                        textBox3.AppendText(YRn + " 20.00 -1\r\n");
                    }
                    
                    /* Try this formula from Salmo (not implimented for v0.5)
                     * x0 = D
                        x1 = D + C /3
                        x2 = D + 2C/3 + B /3
                        x3 = D + C + B + A

                        y0 = H
                        y1 = H + G / 3
                        y2 = H + 2G / 3 + F / 3
                        y3 = H + G + F + E
                     */
                    

                }
                //
                
                //textBox1.AppendText("-------------------------------------------------------------------------------------------------------------------------------------------------------\r\n");
                //textBox1.AppendText("\r\n");
                
                //advance psition pos to end of string with splineLength value
                
                pos = pos + 32 + (48 + (splineLength - 1) * (24));
                
            }   //DefPoints loop
            else
            {
                pos = pos + 32 + (48 + (splineLength - 1) * (24));
            }

            }


            br2.Close();
        }
    }
}
