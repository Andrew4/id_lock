


namespace konzol {
  //--------
  public static class debug_list {
    //--------
    public static System.Collections.SortedList full_log;
    public static System.UInt64 app_start_tick;
    public static System.Boolean debug_support;
    //--------
    public static void init() {
      System.UInt64[] in_put, out_put;
      System.Int64 i1;
      //--
      debug_list.debug_support= true;
      //--
      in_put= new System.UInt64[1];
      in_put[0]= 0;
      out_put= idlock_exec.debug_list(in_put);
      if ((out_put.Length==1) &&
          (out_put[0]==idlock_exec.response_notsupported)) {
        debug_list.debug_support= false;
        return;}
      //--
      debug_list.full_log= new System.Collections.SortedList();
      i1= System.DateTime.UtcNow.Ticks;
      if (i1< 0) i1*= (-1);
      debug_list.app_start_tick= System.Convert.ToUInt64(i1);
      //--
      return;}
    //--------
    public static void app_autoclose_on_error() {
      //--
      if (idlock_xchg.diag_event_flags==0) return;
      if (!debug_list.debug_support) return;
      if (program.loop_activity >0) return;
      program.loop_activity= 1;
      //--
      return;}
    //--------
    public static void close() {
      //--
      if (!debug_list.debug_support) {
        System.Console.WriteLine("No log file this time.");
        return;}
      //--
      debug_list.log_download();
      debug_list.write_log_file();
      //--
      return;}
    //--------
    public static void log_download() {
      System.Text.ASCIIEncoding ascii127_coder;
      System.UInt64 slot_max, slot_next, slot_count;
      System.UInt64[] in_put, out_put;
      System.Int32 i1;
      System.Byte[] t_byte;
      System.String s1, debug_str;
      //--
      ascii127_coder= new System.Text.ASCIIEncoding();
      in_put= new System.UInt64[1];
      //--
      in_put[0]= 0;
      out_put= idlock_exec.debug_list(in_put);
      //--
      if ((out_put.Length==1) &&
          (out_put[0]==idlock_exec.response_notsupported)) {
        debug_list.debug_support= false;
        return;}
      //--
      slot_max= out_put[0];
      slot_next= out_put[1];
      in_put[0]= slot_next;
      out_put= idlock_exec.debug_list(in_put);
      //--
      if ((out_put.Length==1) &&
          (out_put[0]==idlock_exec.response_notsupported)) {
        debug_list.debug_support= false;
        return;}
      //--
      slot_count= slot_max;
      if (out_put[3]==0) slot_count= slot_next;
      //--
      System.Console.WriteLine("Downloading "+ slot_count.ToString()+
          " log records. 100 records / * marker:");
      slot_next= 0;
      slot_max= 100;
      //--
      while(true) {
        //--
        if (slot_next >=slot_count) break;
        //--
        in_put[0]= slot_next;
        out_put= idlock_exec.debug_list(in_put);
        //--
        if ((out_put.Length==1) &&
            (out_put[0]==idlock_exec.response_notsupported)) {
          debug_list.debug_support= false;
          break;}
        //--
        if (out_put[3]==0) break;
        //--
        s1= "";
        for(i1=4; i1< out_put.Length; i1++) {
          t_byte= System.BitConverter.GetBytes(out_put[i1]);
          s1+= ascii127_coder.GetString(t_byte, 0,
              t_byte.Length);
          continue;}
        debug_str= s1.Split('\x00')[0];
        //--
        debug_list.full_log.Add(out_put[3],
            (System.Object)debug_str);
        //--
        slot_next++;
        if (slot_next >=out_put[0]) break;
        //--
        if (slot_next >=slot_max) {
          slot_max+= 100;
          System.Console.Write("*");}
        //--
        continue;}
      System.Console.WriteLine();
      System.Console.WriteLine("Download done.");
      //--
      return;}
    //--------
    public static void write_log_file() {
      System.Reflection.Assembly t_prog;
      System.Uri t_uri;
      System.String t_path;
      System.String debug_str;
      System.IO.StreamWriter out_file;
      System.Int32 i1, i2, i3;
      System.UInt64 t_seq;
      System.DateTime local_time; 
      //--
      if (!debug_list.debug_support) {
        System.Console.WriteLine("No log file this time.");
        return;}
      if (debug_list.full_log.Count==0) {
        System.Console.WriteLine("Empty log.");
        return;}
      System.Console.WriteLine("Writing out "+
          debug_list.full_log.Count.ToString()+ " records.");
      //--
      t_prog= System.Reflection.Assembly.GetExecutingAssembly();
      t_uri= new System.Uri(t_prog.CodeBase);
      t_path= System.IO.Path.GetDirectoryName(t_uri.LocalPath);
      t_path+= System.IO.Path.DirectorySeparatorChar;
      t_path+= "id-test-log-files";
      if (!System.IO.Directory.Exists(t_path))
          System.IO.Directory.CreateDirectory(t_path);
      //--
      local_time= System.DateTime.Now;
      t_path+= System.IO.Path.DirectorySeparatorChar;
      t_path+= "log-";
      t_path+= local_time.Year.ToString("D4");
      t_path+= "_";
      t_path+= local_time.Month.ToString("D2");
      t_path+= "_";
      t_path+= local_time.Day.ToString("D2");
      t_path+= "_";
      t_path+= local_time.Hour.ToString("D2");
      t_path+= "_";
      t_path+= local_time.Minute.ToString("D2");
      if (!System.IO.Directory.Exists(t_path))
          System.IO.Directory.CreateDirectory(t_path);
      //--
      t_path+= System.IO.Path.DirectorySeparatorChar;
      t_path+= "log-"+ debug_list.app_start_tick.ToString("x16");
      t_path+= ".txt";
      out_file= new System.IO.StreamWriter(t_path, false);
      System.Console.WriteLine("Output file: "+ t_path);
      //--
      i2= debug_list.full_log.Count;
      i3= 100;
      for(i1=0; i1< i2; i1++) {
        t_seq= (System.UInt64)debug_list.full_log.GetKey(i1);
        debug_str= (System.String)debug_list.full_log.GetByIndex(i1);
        out_file.WriteLine(debug_str);
        if (i1 >i3) {
          i3+= 100;
          System.Console.Write("*");}
        continue;}
      System.Console.WriteLine();
      //--
      out_file.Close();
      System.Console.WriteLine(i2.ToString()+ " records done.");
      //--
      return;}
    //--------
    }
  //--------
  }

