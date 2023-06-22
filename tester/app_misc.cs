

namespace konzol {
  //--------
  public static class app_misc {
    //--------
    public static System.Int64 ref_time;
    public static System.Random app_rnd;
    //--------
    public static void init() {
      System.TimeSpan t_diff;
      System.DateTime time_2sec_ago;
      System.Int64 t_seed64;
      System.Int32 t_seed32;
      //--
      t_diff= new System.TimeSpan(0, 0, 2);
      time_2sec_ago= System.DateTime.UtcNow.Subtract(t_diff);
      app_misc.ref_time= time_2sec_ago.Ticks;
      //--
      t_seed64= (System.DateTime.UtcNow.Ticks & 0x7fffffff);
      t_seed32= System.Convert.ToInt32(t_seed64);      
      app_misc.app_rnd= new System.Random(t_seed32);
      //--
      return;}
    //--------
    public static System.UInt64 get_time() {
      System.Int64 t_time_ticks;
      System.UInt64 t_time_ms;
      //--
      t_time_ticks= System.DateTime.UtcNow.Ticks;
      t_time_ticks-= app_misc.ref_time;
      t_time_ticks/= System.TimeSpan.TicksPerMillisecond; //->ms
      t_time_ms= System.Convert.ToUInt64(t_time_ticks);
      //--
      return t_time_ms;}
    //--------
    public static System.UInt64 get_rnd() {
      System.UInt64 t_ret_64;
      System.Int32 t_ret_32;
      //--
      t_ret_32= app_misc.app_rnd.Next(1, (idlock_xchg.resource_range+1));
      t_ret_64= System.Convert.ToUInt64(t_ret_32);
      //--
      return t_ret_64;}
    //--------
    public static void key_clear() {
      //--
      while(System.Console.KeyAvailable) System.Console.ReadKey(true);
      //--
      return;}
    //--------
    public static System.Int32 quit_press() {
      System.ConsoleKeyInfo t_key;
      System.Boolean k_enter, k_q;
      System.Int32 t_ret;
      //--
      k_enter= false;
      k_q= false;
      while(true) {
        if (!System.Console.KeyAvailable) break;
        t_key= System.Console.ReadKey(true);
        if (t_key.Key==System.ConsoleKey.Enter) k_enter= true;
        if (t_key.Key==System.ConsoleKey.Q) k_q= true;
        continue;}
      t_ret= 0;
      if (k_enter) t_ret= 1;
      if (k_q) t_ret= 2;
      //--
      return t_ret;}
    //--------
    //--------
    }
  //--------
  }



