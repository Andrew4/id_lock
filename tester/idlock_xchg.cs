
namespace konzol {
  //--------
  public class idlock_xchg_tr {
    //--------
    public System.UInt64 tr_id;
    public System.UInt64 res_id;
    public System.UInt64 time;
    //--------
    public idlock_xchg_tr() {
      //--
      this.tr_id= 0;
      this.res_id= 0;
      this.time= app_misc.get_time();
      //--
      return;}
    //--------
    }
  //--------
  public class idlock_xchg_idx {
    //--------
    public System.UInt64 seq_0;
    public System.UInt64 seq_1;
    public System.UInt64 status;
    public System.UInt64 flags;
    //--------
    public idlock_xchg_idx() {
      //--
      this.seq_0= 0;
      this.seq_1= 0;
      this.status= idlock_exec.nodestatus_P1;
      this.flags= 0;
      //--
      return;}
    //--------
    }
  //--------
  public static class idlock_xchg {
    //--------
    public static idlock_xchg_idx[,] idx_db;
      //Warning: the real index starts from 1
    public static System.UInt64[] idx_delay;
    public static System.Boolean[] remote_z_set;
    public static System.Boolean[] remote_z_clr;
    //--
    public static System.UInt64 diag_in_lck;
    public static System.UInt64 diag_in_idx;
    public static System.UInt64 diag_in_err;
    public static System.UInt64 diag_out_lck;
    public static System.UInt64 diag_out_idx;
    public static System.UInt64 diag_event_flags;
    //--
    public static System.UInt64 local_status;
    public static System.UInt64 local_status_aux;
    public static System.UInt64 local_status_cluster;
    public static System.UInt64 local_delay;
    public static System.UInt64 local_load_last;
    public static System.UInt64 local_load_avg;
    public static System.UInt64 local_load_max;
    //--
    public static System.UInt64 jiffy_multiplier;
    public static System.Int32 portbase_com;
    public static System.String[] idlock_remote_ip;
    private static win_com_ip[] port_com;
    //--
    public static System.UInt64 next_transaction_id;
    public static System.Boolean next_transaction_id_valid;
    //--
    public static System.Int32 cpu_strength;
    public static System.Int32 queue_x_lim;
    public static System.Int32 resource_range;
    //--
    //--------
    public static void init() {
      System.Int32 i1, i2;
      //--
      idlock_xchg.idx_db= new idlock_xchg_idx[
          (win_com.node_count+1),(win_com.node_count+1)];
      idlock_xchg.idx_delay= new System.UInt64[
          (win_com.node_count+1)];
      idlock_xchg.remote_z_set= new System.Boolean[
          (win_com.node_count+1)];
      idlock_xchg.remote_z_clr= new System.Boolean[
          (win_com.node_count+1)];
      for(i1=1; i1< (win_com.node_count+1); i1++) {
        for(i2=1; i2< (win_com.node_count+1); i2++)
            idlock_xchg.idx_db[i1, i2]= new idlock_xchg_idx();
        idlock_xchg.idx_delay[i1]= idlock_exec.idxping_invalid;
        idlock_xchg.remote_z_set[i1]= false;
        idlock_xchg.remote_z_clr[i1]= false;
        continue;}
      //--
      idlock_xchg.diag_in_lck= 0;
      idlock_xchg.diag_in_idx= 0;
      idlock_xchg.diag_in_err= 0;
      idlock_xchg.diag_out_lck= 0;
      idlock_xchg.diag_out_idx= 0;
      idlock_xchg.diag_event_flags= 0;
      //--
      idlock_xchg.local_status= 0;
      idlock_xchg.local_delay= idlock_exec.timer_delay;
      idlock_xchg.local_load_last= 0;
      idlock_xchg.local_load_avg= 0;
      idlock_xchg.local_load_max= 0;
      //--
      idlock_xchg.jiffy_multiplier= 4;  //2..63
        //One jiffy==20 millisecond. Keep this value above the
        //  network ping + cpu lag. The default sets it to 80 ms.
        //If you have a high-performance environment, it is ok to
        //  decrease it.
        //A word of warning here. If you overload the id_lock's
        //  main loop, it shuts down itself to avoid harming the
        //  protocol.
      //--
        //This address interval works up to 15 cluster nodes.
      idlock_xchg.portbase_com= 0xc510; //50448 + 1..
      //--
      idlock_xchg.idlock_remote_ip= new System.String[
          (win_com.node_count+1)];
      for(i1=1; i1< (win_com.node_count+1); i1++)
          idlock_xchg.idlock_remote_ip[i1]= "127.0.0.1";
        //Testing on localhost uses this ip only.
      //--
      idlock_xchg.port_com= new win_com_ip[(win_com.node_count+1)];
      for(i1=1; i1< (win_com.node_count+1); i1++) {
        idlock_xchg.port_com[i1]= new win_com_ip();
        idlock_xchg.port_com[i1].ip=
            idlock_xchg.idlock_remote_ip[i1];
        idlock_xchg.port_com[i1].port= idlock_xchg.portbase_com+ i1;
        continue;}
      //--
      idlock_xchg.next_transaction_id= 1;
      idlock_xchg.next_transaction_id_valid= false;
      //--
      idlock_xchg.cpu_strength= 1;  //1..63
        //1 unit means 15 lock slots on the cluster for parallel
        //  tasks. Slots are split evenly between nodes as you can
        //  see the calculation below. That is the formula in the
        //  protocol binary.
        //If you have a high-performance environment, it is ok to
        //  increase it.
        //Please, read the warning at "idlock_xchg.jiffy_multiplier".
      idlock_xchg.queue_x_lim= (15 * idlock_xchg.cpu_strength);
      idlock_xchg.queue_x_lim/= win_com.node_count;
      if (idlock_xchg.queue_x_lim< 3) idlock_xchg.queue_x_lim= 3;
      if (idlock_xchg.queue_x_lim >250) idlock_xchg.queue_x_lim= 250;
      //idlock_xchg.queue_x_lim= 1;
      //--
      idlock_xchg.resource_range= (15 * idlock_xchg.cpu_strength);
      idlock_xchg.resource_range*= 14;
      idlock_xchg.resource_range/= 10;
        //An extra 40% is enough to keep conflicts on low while
        //  still generating some.
      //--
      return;}
    //--------
    public static System.Boolean first_config() {
      System.UInt64[] t_param, output;
      System.Int32 i1, i2;
      System.String[] s_a;
      System.UInt64[] i_a;
      //--
      t_param= new System.UInt64[4];
      t_param[0]= System.Convert.ToUInt64(win_com.node_count);
      t_param[1]= System.Convert.ToUInt64(win_com.own_idx);
      t_param[2]= idlock_xchg.jiffy_multiplier;
      t_param[3]= System.Convert.ToUInt64(idlock_xchg.cpu_strength);
      output= idlock_exec.call_config_1st(t_param);
      if (output[0]==idlock_exec.response_notsupported) return true;
      if (output[0]!=idlock_exec.response_ok) return false;
      //--
      for(i1=1; i1< (win_com.node_count+1); i1++) {
        s_a= idlock_xchg.port_com[i1].ip.Split('.');
          //"127.0.0.1" -> "127", "0", "0", "1".
        if (s_a.Length!=4) return false;
        i_a= new System.UInt64[4];
        for(i2=0; i2< 4; i2++) {
            //"127", "0", "0", "1" -> 127, 0, 0, 1
          i_a[i2]= System.Convert.ToUInt64(s_a[i2]);
          continue;}
        t_param= new System.UInt64[6];
        t_param[0]= System.Convert.ToUInt64(i1);
        t_param[1]= i_a[0];
        t_param[2]= i_a[1];
        t_param[3]= i_a[2];
        t_param[4]= i_a[3];
        t_param[5]= System.Convert.ToUInt64(
            idlock_xchg.port_com[i1].port);
        output= idlock_exec.call_config_2nd(t_param);
        if (output[0]!=idlock_exec.response_ok) return false;
        continue;}
      //--
      return true;}
    //--------
    public static void query_idx() {
      System.Int32 i1, i2;
      System.UInt64[] t_param, output;
      idlock_xchg_idx t_obj;
      //--
      for(i1=1; i1< (win_com.node_count+1); i1++) {
        t_param= new System.UInt64[1];
        t_param[0]= System.Convert.ToUInt64(i1);
        output= idlock_exec.call_idx_query(t_param);
        if (output.Length==1) throw new System.Exception(
            "query_idx() error report");
        idlock_xchg.idx_delay[i1]= output[0];
        for(i2=0; i2< win_com.node_count; i2++) {
          t_obj= idlock_xchg.idx_db[i1, (i2+1)];
          t_obj.seq_0= output[1+(i2*4)+0];
          t_obj.seq_1= output[1+(i2*4)+1];
          t_obj.status= output[1+(i2*4)+2];
          t_obj.flags= output[1+(i2*4)+3];
            //.lost_ack stays unchanged here
          if ((i1==win_com.own_idx) &&
              ((i2+1)!=win_com.own_idx) &&
              (t_obj.status==idlock_exec.nodestatus_Z1))
              idlock_xchg.remote_z_set[(i2+1)]= true;
          continue;}
        continue;}
      //--
      return;}
    //--------
    public static void update_idx() {
      System.Int32 i1;
      System.UInt64[] t_param, output;
      idlock_xchg_idx t_obj;
      System.Boolean b1;
      //--
      for(i1=1; i1< (win_com.node_count+1); i1++) {
        t_obj= idlock_xchg.idx_db[win_com.own_idx, i1];
        //--
        b1= (idlock_xchg.remote_z_set[i1] &&
             idlock_xchg.remote_z_clr[i1]);
        //--
        t_param= new System.UInt64[4];
        t_param[0]= System.Convert.ToUInt64(i1);
        t_param[1]= t_obj.seq_0;
        t_param[2]= t_obj.seq_1;
        t_param[3]= idlock_exec.lost_ack_empty;
        if (b1) t_param[3]= idlock_exec.lost_ack_mark;
        output= idlock_exec.call_idx_set(t_param);
        if (output[0]!=idlock_exec.response_ok) throw new
            System.Exception("update_idx() error code "+
            output[0].ToString());
        if (b1) {
          idlock_xchg.remote_z_set[i1]= false;
          idlock_xchg.remote_z_clr[i1]= false;}
        continue;}
      //--
      return;}
    //--------
    public static void query_diag() {
      System.UInt64[] output;
      //--
      output= idlock_exec.call_diag_general();
      //--
      idlock_xchg.diag_in_lck= output[0];
      idlock_xchg.diag_in_idx= output[1];
      idlock_xchg.diag_in_err= output[2];
      idlock_xchg.diag_out_lck= output[3];
      idlock_xchg.diag_out_idx= output[4];
      idlock_xchg.diag_event_flags= output[5];
      //--
      return;}
    //--------
    public static void query_local_stat() {
      System.UInt64[] output;
      //--
      output= idlock_exec.call_ctrl_query();
      //--
      if (output.Length< 7) throw new System.Exception(
          "idlock_exec.query_local_stat() error report");
      //--
      idlock_xchg.local_status= output[0];
      idlock_xchg.local_status_aux= output[1];
      idlock_xchg.local_status_cluster= output[2];
      idlock_xchg.local_delay= output[3];
      idlock_xchg.local_load_last= output[4];
      idlock_xchg.local_load_avg= output[5];
      idlock_xchg.local_load_max= output[6];
      //--
      return;}
    //--------
    public static void force_exit() {
      System.UInt64[] p_in, p_out;
      //--
      debug_list.debug_support= false;
      p_in= new System.UInt64[1];
      p_in[0]= idlock_exec.ctrl_switch_forced_exit;
      p_out= idlock_exec.call_ctrl_on_off(p_in);
      if (p_out[0]!=idlock_exec.response_ok) throw new
          System.Exception(
          "impossible control flow idlock_xchg.force_exit()");
      //--
      return;}
    //--------
    public static System.Boolean graceful_exit() {
      System.UInt64[] p_in, p_out;
      //--
      p_in= new System.UInt64[1];
      p_in[0]= idlock_exec.ctrl_switch_graceful_exit;
      p_out= idlock_exec.call_ctrl_on_off(p_in);
      if (p_out[0]==idlock_exec.response_ok) return true;
      if (p_out[0]==idlock_exec.response_wait) return false;
      throw new System.Exception(
          "impossible control flow idlock_xchg.force_exit()");}
    //--------
    //--------
    }
  //--------
  }



