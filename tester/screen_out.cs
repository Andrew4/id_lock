
namespace konzol {
  //--------
  public class screen_out_pktdiag {
    //--------
    public System.UInt64 lck_in;
    public System.UInt64 idx_in;
    public System.UInt64 lck_out;
    public System.UInt64 idx_out;
    public System.UInt64 time;
    //--------
    public screen_out_pktdiag() {
      //--
      this.lck_in= 0;
      this.idx_in= 0;
      this.lck_out= 0;
      this.idx_out= 0;
      this.time= app_misc.get_time();
      //--
      return;}
    //--------
    }
  //--------
  public class screen_out_seq1stat {
    //--------
    public System.UInt64 seq_1;
    public System.UInt64 time;
    //--------
    public screen_out_seq1stat() {
      this.seq_1= 0;
      this.time= app_misc.get_time();
      return;}
    //--------
    }
  //--------
  public static class screen_out {
    //--------
    public static System.Collections.ArrayList diag_list;
    public static System.UInt64 network_use_1sec;
    //--
    public static System.UInt64 last_transaction_id_advertised;
    public static System.UInt64 locks_rejected;
    //--
    public static System.Collections.ArrayList[] tr_idx_list;
    public static System.UInt64 transaction_activity_1sec;
    //--
    public static System.UInt64 avgtime_sum;
    public static System.UInt64 avgtime_count;
    public static System.UInt64 avgtime_min;
    public static System.UInt64 avgtime_max;
    public static System.UInt64 avgtime_avg;
    //--
    public static System.UInt64 screen_next_time;
    public static System.Int32 indicator_chr_val;
    public static System.String indicator_chr;
    //--
    //--------
    public static void init() {
      System.Int32 i1;
      //--
      screen_out.diag_list= new System.Collections.ArrayList();
      screen_out.network_use_1sec= 0;
      //--
      screen_out.last_transaction_id_advertised= 0;
      screen_out.locks_rejected= 0;
      //--
      screen_out.tr_idx_list=  new System.Collections.ArrayList[
          (win_com.node_count+1)];
      for(i1=1; i1< (win_com.node_count+1); i1++)
          screen_out.tr_idx_list[i1]=
          new System.Collections.ArrayList();
      screen_out.transaction_activity_1sec= 0;
      //--
      screen_out.avgtime_sum= 0;
      screen_out.avgtime_count= 0;
      screen_out.avgtime_min= 99999;
      screen_out.avgtime_max= 0;
      screen_out.avgtime_avg= 0;
      //--
      screen_out.screen_next_time= 0;
      screen_out.indicator_chr_val= 0;
      screen_out.indicator_chr= "/";
      //--
      return;}
    //--------network bandwidth kbps
    public static void diagnostic_feed() {
      screen_out_pktdiag obj1, obj2;
      System.Int32 i1;
      System.UInt64 t_time, t_sum;
      //--
      obj1= new screen_out_pktdiag();
      obj1.lck_in= idlock_xchg.diag_in_lck;
      obj1.idx_in= idlock_xchg.diag_in_idx;
      obj1.lck_out= idlock_xchg.diag_out_lck;
      obj1.idx_out= idlock_xchg.diag_out_idx;
      t_time= obj1.time -2000;
      screen_out.diag_list.Add(obj1);
      //--
      while(true) {
        obj1= (screen_out_pktdiag)screen_out.diag_list[0];
        if (obj1.time >=t_time) break;
        screen_out.diag_list.RemoveAt(0);
        continue;}
      //--
      i1= screen_out.diag_list.Count;
      if (i1< 2) return;
      //--
      obj1= (screen_out_pktdiag)screen_out.diag_list[0];
      obj2= (screen_out_pktdiag)screen_out.diag_list[(i1-1)];
      t_time= (obj2.time- obj1.time);
      if (t_time< 1000) return;
      //--
      screen_out.network_use_1sec= 0;
      //--
      t_sum= 0;
      t_sum+= (obj2.lck_in- obj1.lck_in);
      t_sum+= (obj2.lck_out- obj1.lck_out);
      t_sum*= idlock_exec.lck_pkt_len;
      screen_out.network_use_1sec+= t_sum;
      //--
      t_sum= 0;
      t_sum+= (obj2.idx_in- obj1.idx_in);
      t_sum+= (obj2.idx_out- obj1.idx_out);
      t_sum*= idlock_exec.idx_pkt_len;
      screen_out.network_use_1sec+= t_sum;
      //--
      t_sum= screen_out.network_use_1sec;
      t_time= (obj2.time- obj1.time);
      t_sum= ((t_sum * 1000) /t_time); //1-sec avg
      t_sum*= 8; // bit per sec
      t_sum/= 1000; // kbit per sec
      screen_out.network_use_1sec= t_sum;
      //--
      if (screen_out.network_use_1sec >999999)
          screen_out.network_use_1sec= 999999;
      //--
      return;}
    //--------finished transactions per second
    public static void transaction_report_feed() {
      screen_out_seq1stat obj1, obj2;
      System.Int32 i1, i2;
      System.UInt64 t_time, t_sum;
      System.UInt64 t_stat;
      System.Boolean t_drop;
      System.Collections.ArrayList t_list;
      //--
      i1= win_com.own_idx;
      for(i2=1; i2< (win_com.node_count+1); i2++) {
        t_drop= false;
        t_stat= idlock_xchg.idx_db[i1, i2].status;
        if (t_stat!=idlock_exec.nodestatus_N1) t_drop= true;
        t_stat= idlock_xchg.idx_delay[i2];
        if (t_stat >idlock_exec.idxping_warning2) t_drop= true;
        //--
        if (t_drop) {
          screen_out.tr_idx_list[i2].Clear();
          continue;}
        //--
        obj1= new screen_out_seq1stat();
        obj1.seq_1= idlock_xchg.idx_db[i1, i2].seq_1;
        screen_out.tr_idx_list[i2].Add(obj1);
        continue;}
      //--
      for(i2=1; i2< (win_com.node_count+1); i2++) {
        t_list= screen_out.tr_idx_list[i2];
        i1= t_list.Count;
        if (i1==0) continue;
        obj2= (screen_out_seq1stat)t_list[(i1-1)];
        t_time= obj2.time -2000;
        //--
        while(true) {
          obj1= (screen_out_seq1stat)t_list[0];
          if (obj1.time >=t_time) break;
          t_list.RemoveAt(0);
          continue;}
        //--
        continue;}
      //--
      screen_out.transaction_activity_1sec= 0;
      for(i2=1; i2< (win_com.node_count+1); i2++) {
        t_list= screen_out.tr_idx_list[i2];
        i1= t_list.Count;
        if (i1< 2) continue;
        obj1= (screen_out_seq1stat)t_list[0];
        obj2= (screen_out_seq1stat)t_list[(i1-1)];
        t_time= (obj2.time- obj1.time);
        if (t_time< 1000) continue;
        t_sum= (obj2.seq_1- obj1.seq_1);
        t_sum= ((t_sum * 1000) /t_time); //1 sec avg
        screen_out.transaction_activity_1sec+= t_sum;
        continue;}
      //--
      if (screen_out.transaction_activity_1sec >99999)
          screen_out.transaction_activity_1sec= 99999;
      //--
      return;}
    //--------
    public static void transaction_finish_report(
        System.UInt64 start_time) {
      System.UInt64 i1;
      //--
      i1= app_misc.get_time() -start_time;
      if (screen_out.avgtime_max< i1) screen_out.avgtime_max= i1;
      if (screen_out.avgtime_min >i1) screen_out.avgtime_min= i1;
      screen_out.avgtime_count+= 1;
      screen_out.avgtime_sum+= i1;
      screen_out.avgtime_avg=
          (screen_out.avgtime_sum / screen_out.avgtime_count);
      //--
      return;}
    //--------
    public static void screen_refresh() {
      System.Int32 i1;
      System.UInt64 ull1, time_now;
      System.String s_out, s1, s2, s3, s4, s5;
      idlock_xchg_idx t_idx;
      //--
      time_now= app_misc.get_time();
      //--only 10 refresh per second on screen
      if (screen_out.screen_next_time >time_now) return;
      screen_out.screen_next_time= time_now;
      screen_out.screen_next_time+= 100;
      //--
      for(i1=1; i1< (win_com.node_count+1); i1++)
          if (idlock_xchg.remote_z_set[i1])
          idlock_xchg.remote_z_clr[i1]= true;
      System.Console.SetCursorPosition(0,0);
      //----
      //----Line 1
      //----
      s_out= "";
      s_out+= "";
      //--
      //--"[own_idx: 99] "
      s1= System.String.Format("{0,2}",
          win_com.own_idx.ToString("D"));
      s_out+= "[own_idx: "+s1+"] ";
      //--
      //--"[status: Z9ZW] "
      s1= "?";
      ull1= idlock_xchg.local_status;
      if (ull1==idlock_exec.nodestatus_Z1) s1= "Z";
      if (ull1==idlock_exec.nodestatus_P1) s1= "P";
      if (ull1==idlock_exec.nodestatus_C1) s1= "C";
      if (ull1==idlock_exec.nodestatus_R1) s1= "R";
      if (ull1==idlock_exec.nodestatus_N1) s1= "N";
      s2= "?";
      if (idlock_xchg.local_status_aux< 10)
          s2= System.String.Format("{0,1}",
          idlock_xchg.local_status_aux.ToString("D"));
      s3= "?";
      ull1= idlock_xchg.local_status_cluster;
      if (ull1==idlock_exec.nodestatus_Z1) s3= "Z";
      if (ull1==idlock_exec.nodestatus_P1) s3= "P";
      if (ull1==idlock_exec.nodestatus_C1) s3= "C";
      if (ull1==idlock_exec.nodestatus_R1) s3= "R";
      if (ull1==idlock_exec.nodestatus_N1) s3= "N";
      s4= "?";
      ull1= idlock_xchg.local_delay;
      if (ull1==idlock_exec.timer_no_delay) s4= " ";
      if (ull1==idlock_exec.timer_delay) s4= "W";
      s_out+= "[status: "+s1+s2+s3+s4+"] ";
      //--
      //--"[error_flags: 0x000000] "
      s1= idlock_xchg.diag_event_flags.ToString("x6");
      s_out+= "[error_flags: 0x"+s1+"] ";
      //--
      //--"[load: 99 %]"
      s1= System.String.Format("{0,2}",
          idlock_xchg.local_load.ToString("D"));
      s_out+= "[load: "+s1+" %]";
      //--
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //----
      //----Line 2
      //----
      s_out= "";
      s_out+= "";
      //--
      //--"[pkt_lck: 999999999] "
      ull1= idlock_xchg.diag_in_lck +idlock_xchg.diag_out_lck;
      s1= System.String.Format("{0,9}",
          ull1.ToString("D"));
      s_out+= "[pkt_lck: "+s1+"] ";
      //--
      //--"[pkt_idx: 999999999] "
      ull1= idlock_xchg.diag_in_idx +idlock_xchg.diag_out_idx;
      s1= System.String.Format("{0,9}",
          ull1.ToString("D"));
      s_out+= "[pkt_idx: "+s1+"] ";
      //--
      //--"[pkt_err: 999999999]"
      ull1= idlock_xchg.diag_in_err;
      s1= System.String.Format("{0,9}",
          ull1.ToString("D"));
      s_out+= "[pkt_err: "+s1+"] ";
      //--
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //----
      //----Line 3
      //----
      s_out= "";
      s_out+= "";
      //--
      //--"[net_use: 999999 kbps] "
      ull1= screen_out.network_use_1sec;
      s1= System.String.Format("{0,6}",
          ull1.ToString("D"));
      s_out+= "[net_use: "+s1+" kbps] ";
      //--
      //--"[last_trans_id: 0x0000000000000000]"
      ull1= screen_out.last_transaction_id_advertised;
      s1= System.String.Format("{0,16}",
          ull1.ToString("x16"));
      s_out+= "[last_trans_id: 0x"+s1+"]";
      //--
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //----
      //----Line 4
      //----
      s_out= "";
      s_out+= "";
      //--
      //--"[locks_rejected: 999999999] "
      ull1= screen_out.locks_rejected;
      s1= System.String.Format("{0,9}",
          ull1.ToString("D"));
      s_out+= "[locks_rejected: "+s1+"] ";
      //--
      //--"[activity: 99999 t/s] "
      ull1= screen_out.transaction_activity_1sec;
      s1= System.String.Format("{0,5}",
          ull1.ToString("D"));
      s_out+= "[activity: "+s1+" t/s] ";
      //--
      //--"[cl.size: 99]"
      s1= System.String.Format("{0,2}",
          win_com.node_count.ToString("D"));
      s_out+= "[cl.size: "+s1+"]";
      //--
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //----
      //----Line 5
      //----
      s_out= "";
      s_out+= "";
      //--
      //--"[tr.finish min 99999 / avg 99999 / max 99999 ms] "
      ull1= screen_out.avgtime_min;
      s1= System.String.Format("{0,5}",
          ull1.ToString("D"));
      ull1= screen_out.avgtime_avg;
      s2= System.String.Format("{0,5}",
          ull1.ToString("D"));
      ull1= screen_out.avgtime_max;
      s3= System.String.Format("{0,5}",
          ull1.ToString("D"));
      s_out+= "[tr.finish min "+s1+" / avg "+s2+" / max "+s3+" ms] ";
      //--
      //--"[jiffy.mul: 99]"
      s1= System.String.Format("{0,2}",
          idlock_xchg.jiffy_multiplier.ToString("D"));
      s_out+= "[jiffy.mul: "+s1+"]";
      //--
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //----
      //----Line empty
      //----
      s_out= " ";
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //----
      //----Line repeat
      //----
      for(i1=1; i1< (win_com.node_count+1); i1++) {
        s_out= "";
        s_out+= "";
        t_idx= idlock_xchg.idx_db[win_com.own_idx,i1];
        //--
        //--"[99. Z CN a] "
        s1= System.String.Format("{0,2}",
            i1.ToString("D"));
        ull1= System.Convert.ToUInt64(t_idx.status);
        s2= "?";
        if (ull1==idlock_exec.nodestatus_Z1) s2= "Z";
        if (ull1==idlock_exec.nodestatus_P1) s2= "P";
        if (ull1==idlock_exec.nodestatus_C1) s2= "C";
        if (ull1==idlock_exec.nodestatus_R1) s2= "R";
        if (ull1==idlock_exec.nodestatus_N1) s2= "N";
        s3= " ";
        if ((t_idx.flags & 0x01) >0) s3= "C";
        s4= " ";
        if ((t_idx.flags & 0x02) >0) s4= "N";
        ull1= idlock_xchg.idx_delay[i1];
        s5= ull1.ToString("x1");
        s_out+= "["+s1+". "+s2+" "+s3+s4+" "+s5+"] ";
        //--
        //--"[seq_0 / 1: 0x0000000000000000 / 0x0000000000000000]"
        ull1= t_idx.seq_0;
        s1= System.String.Format("{0,16}",
            ull1.ToString("x16"));
        ull1= t_idx.seq_1;
        s2= System.String.Format("{0,16}",
            ull1.ToString("x16"));
        s_out+= "[seq_0 / 1: 0x"+s1+" / 0x"+s2+"]";
        //--
        s_out= System.String.Format("{0,-70}", s_out);
        System.Console.WriteLine(s_out);
        continue;}
      //----
      //----Line empty
      //----
      s_out= " ";
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //----
      //----Line footer
      //----
      //--
      screen_out.indicator_chr_val++;
      if (screen_out.indicator_chr_val >=4)
          screen_out.indicator_chr_val= 0;
      switch(screen_out.indicator_chr_val) {
        case 0: screen_out.indicator_chr= "/"; break;
        case 1: screen_out.indicator_chr= "-"; break;
        case 2: screen_out.indicator_chr= "\\"; break;
        case 3: screen_out.indicator_chr= "|"; break;
        default: screen_out.indicator_chr= "?"; break;}
      //--
      s_out= "["+ screen_out.indicator_chr+
          " Enter: id-lock stop, Q: force quit]";
      if (program.loop_activity!=0)
          s_out= "[Shutting down..                     ]";
      //--
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //----
      //----Line empty
      //----
      s_out= " ";
      s_out= System.String.Format("{0,-70}", s_out);
      System.Console.WriteLine(s_out);
      //--
      return;}
    //--------
    }
  //--------
  }




