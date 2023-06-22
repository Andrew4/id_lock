
//Dev-to-Dev 2023 may: This application aims to test critical
//  functionality issues rather than introducing a full-featured
//  framework. If you are facing such a challenge, stuff here is
//  likely insufficient. Please, be patient. The project is going to
//  improve with time.

namespace konzol {
  //--------
  public static class program {
    //--------
    public static System.Boolean error;
    public static System.Boolean loop_done;
    public static System.Int32 loop_activity;
    public static System.UInt64 loop_next_time;
    public static System.UInt64[] seq_0_low, seq_0_high, seq_1_low;
    public static System.Collections.Queue queue_tr_0, queue_tr_1;
    //--------
    static void Main(string[] args) {
      System.Int32 i1;
      System.String[] sa;
      //--
      program.loop_activity= 0;
        //0: run, 1: graceful exit, 2: forced exit
      //--
      program.error= true;
      try { while(true) {
        //--
        System.Console.WriteLine("app_misc.init()");
        app_misc.init();
        //--
        System.Console.WriteLine("win_com.init()");
        win_com.init();
        //--
        System.Console.WriteLine("idlock_exec.init()");
        idlock_exec.init();  //uses win_com
        //--
        System.Console.WriteLine("idlock_xchg.init()");
        idlock_xchg.init();  //uses idlock_exec, win_com
        //--
        System.Console.WriteLine("win_com.udp_port_bind()");
        if (!win_com.udp_port_bind()) break;
          //uses win_com+ idlock_xchg init
        //--
        System.Console.WriteLine("screen_out.init()");
        screen_out.init();  //uses win_com, app_misc
        //--
        System.Console.WriteLine("idlock_xchg.first_config()");
        if (!idlock_xchg.first_config()) break;
          //uses udp_port_bind, idlock_xchg
        //--
        System.Console.WriteLine("debug_list.init()");
        debug_list.init();
          //uses idlock_exec
        //--
        System.Console.WriteLine("app_misc.key_clear()");
        app_misc.key_clear();
        //--
        System.Console.Clear();
        program.loop_next_time= app_misc.get_time();
        program.loop_done= false;
        program.seq_0_low= new System.UInt64[(win_com.node_count+1)];
        program.seq_0_high=
            new System.UInt64[(win_com.node_count+1)];
        program.seq_1_low= new System.UInt64[(win_com.node_count+1)];
        program.queue_tr_0= new System.Collections.Queue();
        program.queue_tr_1= new System.Collections.Queue();
        while(true) {
          main_loop_Z();
          if (program.loop_done) break;
          main_loop_P();
          if (program.loop_done) break;
          main_loop_C();
          if (program.loop_done) break;
          main_loop_R();
          if (program.loop_done) break;
          main_loop_N_delay();
          if (program.loop_done) break;
          main_loop_N();
          break;}
        //--
        program.error= false;
        //--
        break;}} catch (System.Exception e_msg) {
          sa= e_msg.ToString().Split('\n');
          for(i1=0; i1< sa.Length; i1++)
              System.Console.WriteLine(sa[i1]);}
      //--
      try {
        System.Console.Clear();
        System.Console.WriteLine("Closing main_loop: debug");
        debug_list.close();
        if (program.loop_activity< 2) {
          System.Console.WriteLine("Sending force_exit to id-lock");
          idlock_xchg.force_exit();}
        System.Console.WriteLine("Closing main_loop: UDP port");
        win_com.udp_port_close();
        } catch (System.Exception e_msg) {
        sa= e_msg.ToString().Split('\n');
        for(i1=0; i1< sa.Length; i1++)
            System.Console.WriteLine(sa[i1]);}
      //--
      app_misc.key_clear();
      System.Console.WriteLine("Hit enter to exit.");
      System.Console.ReadLine();
      //--
      return;}
    //--------
    public static void main_loop_common() {
      System.UInt64 time_now;
      System.Int32 i1;
      System.Int32 idx1, idx2;
      System.UInt64 u64_0, u64_1;
      //--
      while(true) {
        //----waiting for time limit
        time_now= app_misc.get_time();
        if (program.loop_next_time >time_now) {
          System.Threading.Thread.Sleep(5);
          continue;}
        program.loop_next_time= 20;
        program.loop_next_time+= time_now;
        //----check for exit intention
        i1= app_misc.quit_press();
        if (i1 >program.loop_activity) program.loop_activity= i1;
        debug_list.app_autoclose_on_error();
        if (program.loop_activity==2) {
          idlock_xchg.force_exit();
          program.loop_done= true;
          break;}
        if ((program.loop_activity==1) &&
            (idlock_xchg.graceful_exit())) {
          program.loop_done= true;
          break;}
        //----cache from id_locl
        idlock_xchg.query_idx();
        idlock_xchg.query_diag();
        idlock_xchg.query_local_stat();
        //----sending data to the display module
        screen_out.diagnostic_feed();
        screen_out.transaction_report_feed();
        //----seq_x statistics filter
        for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
          program.seq_0_low[idx2]= System.UInt64.MaxValue;
          program.seq_0_high[idx2]= 0;
          program.seq_1_low[idx2]= System.UInt64.MaxValue;
          continue;}
        for(idx1=1; idx1< (win_com.node_count+1); idx1++) {
          i1= 0;
          u64_1= idlock_xchg.idx_db[win_com.own_idx,idx1].status;
          if (u64_1==idlock_exec.nodestatus_C1) i1++;
          if (u64_1==idlock_exec.nodestatus_R1) i1++;
          if (u64_1==idlock_exec.nodestatus_N1) i1++;
          if (idlock_xchg.idx_delay[idx1]< idlock_exec.idxping_fail1)
              i1++;
          if (i1< 2) continue;
          for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
            u64_0= idlock_xchg.idx_db[idx1,idx2].seq_0;
            u64_1= idlock_xchg.idx_db[idx1,idx2].seq_1;
            if (program.seq_0_low[idx2] >u64_0)
                program.seq_0_low[idx2]= u64_0;
            if (program.seq_0_high[idx2]< u64_0)
                program.seq_0_high[idx2]= u64_0;
            if (program.seq_1_low[idx2] >u64_1)
                program.seq_1_low[idx2]= u64_1;
            continue;}
          continue;}
        //----display the data
        screen_out.screen_refresh();
        //----
        break;}
      //--
      return;}
    //--------
    public static void main_loop_Z() {
      System.UInt64[] p_in, p_out;
      //--
      while(true) {
        program.main_loop_common();
        if (idlock_xchg.local_status!=idlock_exec.nodestatus_Z1)
            break;
        if (program.loop_done) break;
        if (idlock_xchg.local_delay==idlock_exec.timer_delay)
            continue;
        p_in= new System.UInt64[1];
        p_in[0]= idlock_exec.ctrl_switch_P1;
        p_out= idlock_exec.call_ctrl_on_off(p_in);
        if (p_out[0]==idlock_exec.response_wait) continue;
        break;}
      //--
      return;}
    //--------
    public static void main_loop_P() {
      System.UInt64[] p_in, p_out;
      System.Int32 idx2;
      //--
      while(true) {
        program.main_loop_common();
        if (idlock_xchg.local_status!=idlock_exec.nodestatus_P1)
            break;
        if (program.loop_done) break;
        //--
        for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_0=
              program.seq_0_high[idx2];
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_1=
              program.seq_0_low[idx2];
          continue;}
        idlock_xchg.update_idx();
        //--
        if (idlock_xchg.local_delay==idlock_exec.timer_delay)
            continue;
        p_in= new System.UInt64[1];
        p_in[0]= idlock_exec.ctrl_switch_C1R1;
        p_out= idlock_exec.call_ctrl_on_off(p_in);
        if (p_out[0]==idlock_exec.response_wait) continue;
        break;}
      //--
      return;}
    //--------
    public static void main_loop_C() {
      System.Int32 idx2;
      //--
      while(true) {
        program.main_loop_common();
        if (idlock_xchg.local_status!=idlock_exec.nodestatus_C1)
            break;
        if (program.loop_done) break;
        //--
        for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_0=
              program.seq_0_high[idx2];
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_1=
              program.seq_0_low[idx2];
          continue;}
        idlock_xchg.update_idx();
        //--
        continue;}
      //--
      return;}
    //--------
    public static void main_loop_R() {
      System.Int32 idx2;
      //--
      while(true) {
        program.main_loop_common();
        if (idlock_xchg.local_status!=idlock_exec.nodestatus_R1)
            break;
        if (program.loop_done) break;
        //--
        for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_0=
              program.seq_0_high[idx2];
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_1=
              program.seq_0_low[idx2];
          continue;}
        idlock_xchg.update_idx();
        //--
        continue;}
      //--
      return;}
    //--------
    public static void main_loop_N_delay() {
      System.Int32 idx2;
      //--
      while(true) {
        program.main_loop_common();
        if (idlock_xchg.local_status!=idlock_exec.nodestatus_N1)
            break;
        if (idlock_xchg.local_delay!=idlock_exec.timer_delay)
            break;
        if (program.loop_done) break;
        //--
        for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_0=
              program.seq_0_high[idx2];
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_1=
              program.seq_0_low[idx2];
          continue;}
        idlock_xchg.update_idx();
        //--
        continue;}
      //--
      return;}
    //--------
    public static void main_loop_N() {
      System.Int32 idx2, i1;
      System.UInt64 u64_1, trans_id_last, seq_1_limit;
      System.UInt64[] p_in, p_out;
      idlock_xchg_tr tr_o;
      //--
      if (idlock_xchg.local_status!=idlock_exec.nodestatus_N1)
          return;
      if (idlock_xchg.local_status_aux >2)
          idlock_xchg.next_transaction_id=
          program.seq_0_high[win_com.own_idx]+ 1;
        //_aux 3..6 means this is a reconnected node and not a
        //  cold-started one
      //--
      while(true) {
        program.main_loop_common();
        if (idlock_xchg.local_status!=idlock_exec.nodestatus_N1)
            break;
        if (program.loop_done) break;
        //--
        for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_0=
              program.seq_0_high[idx2];
          idlock_xchg.idx_db[win_com.own_idx,idx2].seq_1=
              program.seq_0_low[idx2];
          continue;}
        idlock_xchg.update_idx();
        //--
        //----generating virtual transactions here
        while(true) {
          if (program.loop_activity >0) break;
          i1= (program.queue_tr_0.Count+ program.queue_tr_1.Count);
          if (i1 >=idlock_xchg.queue_x_lim) break;
          tr_o= new idlock_xchg_tr();
          tr_o.tr_id= idlock_xchg.next_transaction_id;
          p_in= new System.UInt64[2];
          while(true) {
            tr_o.res_id= app_misc.get_rnd();
            p_in[0]= tr_o.res_id;
            p_in[1]= tr_o.tr_id;
            p_out= idlock_exec.call_lock_set(p_in);
            u64_1= p_out[0];
            if (u64_1==idlock_exec.response_reserved) continue;
            break;}
          if (u64_1==idlock_exec.response_wait) break;
          if (u64_1==idlock_exec.response_notsupported) break;
          if (u64_1!=idlock_exec.response_ok)
              throw new System.Exception(
              "impossible control flow Nx_rdy st_1");
          idlock_xchg.next_transaction_id++;
          program.queue_tr_0.Enqueue((System.Object)tr_o);
          continue;}
        //--
        //----collecting successfully locked IDs
        trans_id_last= 0;
        while(true) {
          //--
          if (program.queue_tr_0.Count==0) break;
          tr_o= (idlock_xchg_tr)program.queue_tr_0.Peek();
          p_in= new System.UInt64[1];
          p_in[0]= tr_o.tr_id;
          p_out= idlock_exec.call_lock_query(p_in);
          u64_1= p_out[0];
          //--
          if (u64_1==idlock_exec.response_notsupported) break;
          if (u64_1==idlock_exec.lockstat_does_not_exist) {
            trans_id_last= tr_o.tr_id;
            screen_out.locks_rejected++;
            tr_o= (idlock_xchg_tr)program.queue_tr_0.Dequeue();
            continue;}
          if (u64_1==idlock_exec.lockstat_lvl2_done) {
            tr_o= (idlock_xchg_tr)program.queue_tr_0.Dequeue();
            trans_id_last= tr_o.tr_id;
            program.queue_tr_1.Enqueue((System.Object)tr_o);
            continue;}
          //--
          break;}
        //--
        //----sending notification to remote nodes
        if (trans_id_last >0) {
          //--Some internal diagnostic first
          u64_1= screen_out.last_transaction_id_advertised;
          if (u64_1 >trans_id_last) {
            program.custom_report_1(trans_id_last,
                screen_out.last_transaction_id_advertised,
                program.seq_0_low, program.seq_0_high,
                program.seq_1_low);
            throw new System.Exception(
              "impossible control flow Nx_rdy st_2 #1");}
          screen_out.last_transaction_id_advertised= trans_id_last;
          i1= win_com.own_idx;
          if (program.seq_0_high[i1] >trans_id_last) {
            program.custom_report_1(trans_id_last,
                screen_out.last_transaction_id_advertised,
                program.seq_0_low, program.seq_0_high,
                program.seq_1_low);
            throw new System.Exception(
                "impossible control flow Nx_rdy st_2 #2");}
          //--notification
          idlock_xchg.idx_db[i1, i1].seq_0= trans_id_last;
          idlock_xchg.update_idx();
          //--
          }
        //--
        //----reporting finished transactions
        i1= win_com.own_idx;
        seq_1_limit= idlock_xchg.idx_db[i1, i1].seq_1;
        while(true) {
          if (program.queue_tr_1.Count==0) break;
          tr_o= (idlock_xchg_tr)program.queue_tr_1.Peek();
          if (tr_o.tr_id >seq_1_limit) break;
          tr_o= (idlock_xchg_tr)program.queue_tr_1.Dequeue();
          screen_out.transaction_finish_report(tr_o.time);
          continue;}
        //--
        continue;}
      //--
      return;}
    //--------
    //--------
/*


        //----process remote_Z flags
        idlock_xchg.update_idx();

    public static void main_loop() {
      System.UInt64 time_now, trans_id_last, seq_1_limit;
      System.Boolean loop_done;
      System.UInt64[] p_in, p_out;
      System.UInt64[] seq_0_low, seq_0_high, seq_1_low;
      System.Int32 idx1, idx2;
      System.Int32 i1;
      System.UInt64 u64_1;
      System.Collections.Queue queue_tr_0, queue_tr_1;
      idlock_xchg_tr tr_o;
      //--
      loop_next_time= 0;
      trans_id_last= 0;
      queue_tr_0= new System.Collections.Queue();
      queue_tr_1= new System.Collections.Queue();
      seq_0_low= new System.UInt64[(win_com.node_count+1)];
      seq_0_high= new System.UInt64[(win_com.node_count+1)];
      seq_1_low= new System.UInt64[(win_com.node_count+1)];
      //--
      while(true) {
        //----
        //----"mainloop" / "start"
        time_now= app_misc.get_time();
        if (loop_next_time >time_now) {
          System.Threading.Thread.Sleep(5);
          continue;}
        loop_next_time= 30;
        loop_next_time+= time_now;
        loop_counter++;
        //----
        //----"mainloop" / "screen_report" + "remote_z_clr"
        screen_out.screen_refresh();
        //----
        //----"mainloop" / "user_exit"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / user_exit");
        i1= app_misc.quit_press();
        if (i1 >program.loop_activity) program.loop_activity= i1;
        debug_list.app_autoclose_on_error();
        //----
        //----"mainloop" / "id_lock_exit"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / id_lock_exit");
        loop_done= false;
        while(true) {
          if (program.loop_activity==0) break;
          p_in= new System.UInt64[1];
          if (program.loop_activity==2) {
            idlock_xchg.force_exit();
            loop_done= true;
            break;}
          if (idlock_xchg.graceful_exit()) loop_done= true;
          break;}
        if (loop_done) break;
        //----
        //----"mainloop" / "idlock_xchg_cache" + "remote_z_set"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / idlock_xchg_cache + remote_z_set");
        idlock_xchg.query_idx();
        idlock_xchg.query_diag();
        idlock_xchg.query_local_stat();
        screen_out.diagnostic_feed();
        screen_out.transaction_report_feed();
        //----
        //----"mainloop" / "seq_x_read"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / seq_x_read");
        for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
          seq_0_low[idx2]= System.UInt64.MaxValue;
          seq_0_high[idx2]= 0;
          seq_1_low[idx2]= System.UInt64.MaxValue;
          continue;}
        for(idx1=1; idx1< (win_com.node_count+1); idx1++) {
          i1= 0;
          u64_1= idlock_xchg.idx_db[win_com.own_idx,idx1].status;
          if (u64_1==idlock_exec.nodestatus_C1) i1++;
          if (u64_1==idlock_exec.nodestatus_R1) i1++;
          if (u64_1==idlock_exec.nodestatus_N1) i1++;
          if (idlock_xchg.idx_delay[idx1]< idlock_exec.idxping_fail1)
              i1++;
          if (i1< 2) continue;
          for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
            if (seq_0_low[idx2] >idlock_xchg.idx_db[idx1,idx2].seq_0)
                seq_0_low[idx2]= idlock_xchg.idx_db[idx1,idx2].seq_0;
            if (seq_0_high[idx2]<idlock_xchg.idx_db[idx1,idx2].seq_0)
                seq_0_high[idx2]=idlock_xchg.idx_db[idx1,idx2].seq_0;
            if (seq_1_low[idx2] >idlock_xchg.idx_db[idx1,idx2].seq_1)
                seq_1_low[idx2]= idlock_xchg.idx_db[idx1,idx2].seq_1;
            continue;}
          continue;}
        //----
        //----"mainloop" / "idx_report" + "remote_z_exec"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / idx_report + remote_z_exec");
        idlock_xchg.update_idx();
        //----
        //----"mainloop" / "mainstat"
        loop_done= false;
        //----
        //----"mainloop" / "mainstat" / "Zx"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / mainstat / Zx");
        while(true) {
          if (idlock_xchg.local_status!=idlock_exec.nodestatus_Z1)
              break;
          loop_done= true;
          if ((program.started_once) && (program.loop_activity==0))
              program.loop_activity= 1;
            //About the program.started_once:
            //  The id-lock can restart from Zx, but the tester
            //  can not. Simply, I did not design it for that.
          if (program.loop_activity >0) break;
          if (idlock_xchg.local_delay==idlock_exec.timer_delay)
              break;
          p_in= new System.UInt64[1];
          p_in[0]= idlock_exec.ctrl_switch_P1;
          p_out= idlock_exec.call_ctrl_on_off(p_in);
          break;}
        if (loop_done) continue;
        program.started_once= true;
        //----
        //----"mainloop" / "mainstat" / "Px"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / mainstat / Px");
        while(true) {
          if (idlock_xchg.local_status!=idlock_exec.nodestatus_P1)
              break;
          loop_done= true;
          if (program.loop_activity >0) break;
          if (idlock_xchg.local_delay==idlock_exec.timer_delay)
              break;
          if (!idlock_xchg.next_transaction_id_valid) {
            idlock_xchg.next_transaction_id_valid= true;
            u64_1= idlock_xchg.next_transaction_id;
            if (u64_1< seq_0_high[win_com.own_idx])
              u64_1= seq_0_high[win_com.own_idx]+ 1;
            idlock_xchg.next_transaction_id= u64_1; }
          p_in= new System.UInt64[1];
          p_in[0]= idlock_exec.ctrl_switch_C1R1;
          idlock_exec.call_ctrl_on_off(p_in);
          break;}
        if (loop_done) continue;
        //----
        //----"mainloop" / "mainstat" / "C/R/N"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / mainstat / C/R/N");
        while(true) {
          while(true) {
            if (idlock_xchg.local_status==idlock_exec.nodestatus_C1)
              break;
            if (idlock_xchg.local_status==idlock_exec.nodestatus_R1)
              break;
            if (idlock_xchg.local_status==idlock_exec.nodestatus_N1)
              break;
            throw new System.Exception("main_loop() mainloop / "+
                "mainstat / C/R/N: unknown node status ??? ");}
          if (idlock_xchg.local_status!=idlock_exec.nodestatus_N1)
              loop_done= true;
          if (idlock_xchg.local_delay==idlock_exec.timer_delay)
              loop_done= true;
          for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
            idlock_xchg.idx_db[win_com.own_idx,idx2].seq_0=
                seq_0_high[idx2];
            idlock_xchg.idx_db[win_com.own_idx,idx2].seq_1=
                seq_0_low[idx2];
            p_in= new System.UInt64[4];
            p_in[0]= System.Convert.ToUInt64(idx2);
            p_in[1]= seq_0_high[idx2]; //seq_0 above
            p_in[2]= seq_0_low[idx2]; //seq_1 above
            p_in[3]= System.Convert.ToUInt64(
                idlock_exec.lost_ack_empty);
            idlock_exec.call_idx_set(p_in);
            continue;}
          break;}
        if (loop_done) continue;
        //----
        //----"mainloop" / "mainstat" / "Nx_rdy" / "st_1"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / mainstat / Nx_rdy / st_1");
        while(true) {
          if (program.loop_activity >0) break;
          i1= (queue_tr_0.Count+ queue_tr_1.Count);
          if (i1 >=idlock_xchg.queue_x_lim) break;
          tr_o= new idlock_xchg_tr();
          tr_o.tr_id= idlock_xchg.next_transaction_id;
          u64_1= idlock_exec.response_notsupported;
          p_in= new System.UInt64[2];
          while(true) {
            tr_o.res_id= app_misc.get_rnd();
            p_in[0]= tr_o.res_id;
            p_in[1]= tr_o.tr_id;
            p_out= idlock_exec.call_lock_set(p_in);
            u64_1= p_out[0];
            if (u64_1==idlock_exec.response_reserved) continue;
            break;}
          if (u64_1==idlock_exec.response_wait) break;
          if (u64_1==idlock_exec.response_notsupported) break;
          if (u64_1!=idlock_exec.response_ok)
              throw new System.Exception(
              "impossible control flow Nx_rdy st_1");
          idlock_xchg.next_transaction_id++;
          queue_tr_0.Enqueue((System.Object)tr_o);
          continue;}
        //----
        //----"mainloop" / "mainstat" / "Nx_rdy" / "st_2"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / mainstat / Nx_rdy / st_2");
        trans_id_last= 0;
        while(true) {
          //--
          if (queue_tr_0.Count==0) break;
          tr_o= (idlock_xchg_tr)queue_tr_0.Peek();
          p_in= new System.UInt64[1];
          p_in[0]= tr_o.tr_id;
          p_out= idlock_exec.call_lock_query(p_in);
          u64_1= p_out[0];
          //--
          if (u64_1==idlock_exec.response_notsupported) break;
          if (u64_1==idlock_exec.lockstat_does_not_exist) {
            trans_id_last= tr_o.tr_id;
            screen_out.locks_rejected++;
            tr_o= (idlock_xchg_tr)queue_tr_0.Dequeue();
            continue;}
          if (u64_1==idlock_exec.lockstat_lvl2_done) {
            tr_o= (idlock_xchg_tr)queue_tr_0.Dequeue();
            trans_id_last= tr_o.tr_id;
            queue_tr_1.Enqueue((System.Object)tr_o);
            continue;}
          //--
          break;}
        if (trans_id_last >0) {
          //--
          u64_1= screen_out.last_transaction_id_advertised;
          if (u64_1 >trans_id_last) {
            program.custom_report_1(trans_id_last,
                screen_out.last_transaction_id_advertised,
                seq_0_low, seq_0_high, seq_1_low);
            throw new System.Exception(
              "impossible control flow Nx_rdy st_2 #1");}
          screen_out.last_transaction_id_advertised= trans_id_last;
          i1= win_com.own_idx;
          if (seq_0_high[i1] >trans_id_last) {
            program.custom_report_1(trans_id_last,
                screen_out.last_transaction_id_advertised,
                seq_0_low, seq_0_high, seq_1_low);
            throw new System.Exception(
                "impossible control flow Nx_rdy st_2 #2");}
          //--
          p_in= new System.UInt64[4];
          p_in[0]= System.Convert.ToUInt64(i1);
          p_in[1]= trans_id_last;
          p_in[2]= idlock_xchg.idx_db[i1, i1].seq_1;
          p_in[3]= idlock_exec.lost_ack_empty;
          p_out= idlock_exec.call_idx_set(p_in);
          if (p_out[0]!=idlock_exec.response_ok)
              throw new System.Exception(
              "impossible control flow Nx_rdy st_2 #3");
          //--
          }
        //----
        //----"mainloop" / "mainstat" / "Nx_rdy" / "st_3"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / mainstat / Nx_rdy / st_3");
        i1= win_com.own_idx;
        seq_1_limit= idlock_xchg.idx_db[i1, i1].seq_1;
        while(true) {
          if (queue_tr_1.Count==0) break;
          tr_o= (idlock_xchg_tr)queue_tr_1.Peek();
          if (tr_o.tr_id >seq_1_limit) break;
          tr_o= (idlock_xchg_tr)queue_tr_1.Dequeue();
          screen_out.transaction_finish_report(tr_o.time);
          continue;}
        //----
        //----"mainloop" / "end"
          //System.Console.WriteLine(
          //    System.String.Format("{0,6}",
          //    loop_counter.ToString("D"))+ " "+
          //    "mainloop / end");
        continue;}
      //--
      return;}
*/
    //--------
    public static void custom_report_1(
        System.UInt64 trans_id_last,
        System.UInt64 last_transaction_id_advertised,
        System.UInt64[] seq_0_low,
        System.UInt64[] seq_0_high,
        System.UInt64[] seq_1_low) {
      System.Int32 idx1, idx2;
      System.String s1, s_l, s_e;
      //--
      s_l= "-------------------------------------------------------";
      s_e= "                ";
      System.Console.WriteLine();
      System.Console.WriteLine(s_l);
      System.Console.WriteLine("    "+trans_id_last.ToString("x16"));
      System.Console.WriteLine("    "+
          last_transaction_id_advertised.ToString("x16"));
      System.Console.WriteLine(s_l);
      for(idx2=1; idx2< (win_com.node_count+1); idx2++) {
        for(idx1=1; idx1< (win_com.node_count+1); idx1++) {
          s1= "";
          s1+= idx1.ToString("D")+ "."+ idx2.ToString("D")+ " ";
          s1+= idlock_xchg.idx_db[idx1,idx2].seq_0.ToString("x16");
          s1+= " "+ s_e+ " ";
          s1+= idlock_xchg.idx_db[idx1,idx2].seq_1.ToString("x16");
          System.Console.WriteLine(s1);
          continue;}
        s1= "";
        s1+= "["+ idx2.ToString("D")+ "] ";
        s1+= seq_0_low[idx2].ToString("x16")+ " ";
        s1+= seq_0_high[idx2].ToString("x16")+ " ";
        s1+= seq_1_low[idx2].ToString("x16")+ " ";
        System.Console.WriteLine(s1);
        System.Console.WriteLine(s_l);
        continue;}
      /* ---------------------------------------------------
      -------------
      trans_id_last
      trans_id_last_advertised
      -------------
      1.1 seq_0                seq_1
      2.1 seq_0                seq_1
      3.1 seq_0                seq_1
      [1] seq_0_low seq_0_high seq_1_low
      --------------
      1.2 seq_0                seq_1
      2.2 seq_0                seq_1
      3.2 seq_0                seq_1
      [2] seq_0_low seq_0_high seq_1_low
      --------------
      1.3 seq_0                seq_1
      2.3 seq_0                seq_1
      3.3 seq_0                seq_1
      [3] seq_0_low seq_0_high seq_1_low
      --------------
      --------------------------------------------------- */
      //--
      return;}
    //--------
    //--------
    }
  //--------
  }





