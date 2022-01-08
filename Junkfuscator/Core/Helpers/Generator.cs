using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkfuscator.Core.Helpers
{
    public static class Generator
    {
        internal static Random rdm = new Random();
        internal static string RandomString(int length, int type)
        {
            if(type == 0)
                return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length).Select(s => s[rdm.Next(s.Length)]).ToArray());
            else if(type == 1)
                return new string(Enumerable.Repeat("A̸͖̦̽̒̓͜B̸̟̞̀̈́͊͜Ć̵̪͍̞͘͝D̸̢͓͑͐́E̴̡̠̟̿̓͋F̴̡̢̝̀͠͝G̴͕͕͇͑̓͘H̴͔̻̟͊̒̔I̸̙͖̺͌̒̒J̴̻̠͒͆͠Ḱ̸̦͍͔̾͝Ĺ̸͉̪̠̒̐M̴͖͍̠̔͛̒N̵̘̠̟̒́͐Ö̴͓̫͍́̾̚P̴̪͔͚̽̈́̈́Q̵̻͎̝̓̔͘R̴͚̠͉̈́̓̐S̴̡͔̔͋͛T̸̡͙̻̓͒͝U̸͓̻̘͋̚V̵̡̫̈́̔̐W̵͙̦͛̕͜͝X̵̢̡̪͋̚Y̵̟͚̞͌̕͝Z̸̠̪͇̐̒͝a̵͓̞͉̐͐͝b̵̙̪̻̈́͋͠c̴̡̦̈́͐̓d̴̢͔͔͛̈́̓e̴̺͚̼̓͋͠f̵̙̞͕͐͑͘g̵͇̟͖̔͐̾h̵͍͔͔͌͘i̵͉͉͆͜͠j̵̡̻̘̀̀͝k̴̞͍͆͛͊l̵̡̙͖͛̓m̵̢͇̼͐̀n̴͓͙̓͆̕͜o̵̺͕͛͒͛p̴͔͔̺̾̐͝q̸͔̪̞̾̒͐r̸̡̻̠͐͘͘s̴͚͙̽͋̾͜t̸͍͚͇̀͑̓u̸͓̼͉̐͘͝v̸̡͎̪̾͑̚w̸̟̼̓̚͠x̴̙͍̟͑̚͝y̵͚̦̺͒̾z̴͚̠̈́̐͋0̴̻̼̒͋͒1̵͖͉̫̐͝2̴̠͉̞͆́̕3̵͙͙̙̐̾͑4̵̢̟͙͋͒͘5̵̦͇͙̐͝͠6̵̞͔̻͐̒7̸̘͎̝͛̓̐8̸̻̻̼͊̔̔9̸̢̪̝͛̈́͐", length).Select(s => s[rdm.Next(s.Length)]).ToArray());

            return string.Empty;
        }
    }
}
