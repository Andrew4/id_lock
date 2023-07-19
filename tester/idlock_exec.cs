

namespace konzol {
  //--------
  public static class idlock_exec {
    //--------
      //Status codes
    public static System.UInt64 nodestatus_Z1;
    public static System.UInt64 nodestatus_P1;
    public static System.UInt64 nodestatus_C1;
    public static System.UInt64 nodestatus_R1;
    public static System.UInt64 nodestatus_N1;
    //--
      //Command codes
    public static System.UInt64 cmd_diag_general;
    public static System.UInt64 cmd_config_1st;
    public static System.UInt64 cmd_config_2nd;
    public static System.UInt64 cmd_ctrl_on_off;
    public static System.UInt64 cmd_ctrl_query;
    public static System.UInt64 cmd_idx_query;
    public static System.UInt64 cmd_idx_set;
    public static System.UInt64 cmd_lock_set;
    public static System.UInt64 cmd_lock_query;
    public static System.UInt64 cmd_res_id_lookup;
    public static System.UInt64 cmd_tr_id_lookup;
    public static System.UInt64 cmd_debug_list;
    //--
      //Command control codes
    public static System.UInt64 ctrl_switch_P1;
    public static System.UInt64 ctrl_switch_C1R1;
    public static System.UInt64 ctrl_switch_graceful_exit;
    public static System.UInt64 ctrl_switch_forced_exit;
    //--
      //Command return codes
    public static System.UInt64 response_notsupported;
    public static System.UInt64 response_error;
    public static System.UInt64 response_ok;
    public static System.UInt64 response_wait;
    public static System.UInt64 response_reserved;
    public static System.UInt64 response_not_found;
    //--
      //Local node / timer status codes
    public static System.UInt64 timer_no_delay;
    public static System.UInt64 timer_delay;
    //--
      //Remote Z acknowledge values
    public static System.UInt64 lost_ack_empty;
    public static System.UInt64 lost_ack_mark;
    //--
      //Lock status codes
    public static System.UInt64 lockstat_invalid;
    public static System.UInt64 lockstat_does_not_exist;
    public static System.UInt64 lockstat_lvl1_in_progress;
    public static System.UInt64 lockstat_lvl1_done;
    public static System.UInt64 lockstat_lvl2_in_progress;
    public static System.UInt64 lockstat_lvl2_stable;
    public static System.UInt64 lockstat_lvl2_done;
    //--
      //Idx keep-alive time limits
    public static System.UInt64 idxping_repeat0;
    public static System.UInt64 idxping_repeat2;
    public static System.UInt64 idxping_repeat3;
    public static System.UInt64 idxping_warning1;
    public static System.UInt64 idxping_warning2;
    public static System.UInt64 idxping_fail1;
    public static System.UInt64 idxping_fail2;
    public static System.UInt64 idxping_fail9;
    public static System.UInt64 idxping_fail20;
    public static System.UInt64 idxping_invalid;
    //--
    public static System.UInt64 idx_pkt_len;
    public static System.UInt64 lck_pkt_len;
    //--------
    public static void init() {
      //--
      idlock_exec.nodestatus_Z1= 0x01;
      idlock_exec.nodestatus_P1= 0x02;
      idlock_exec.nodestatus_C1= 0x03;
      idlock_exec.nodestatus_R1= 0x04;
      idlock_exec.nodestatus_N1= 0x05;
      //--
      idlock_exec.cmd_diag_general= 0x00;
      idlock_exec.cmd_config_1st= 0x01;
      idlock_exec.cmd_config_2nd= 0x02;
      idlock_exec.cmd_ctrl_on_off= 0x11;
      idlock_exec.cmd_ctrl_query= 0x12;
      idlock_exec.cmd_idx_query= 0x21;
      idlock_exec.cmd_idx_set= 0x22;
      idlock_exec.cmd_lock_set= 0x31;
      idlock_exec.cmd_lock_query= 0x32;
      idlock_exec.cmd_res_id_lookup= 0x33;
      idlock_exec.cmd_tr_id_lookup= 0x34;
      idlock_exec.cmd_debug_list= 0x41;
      //--
      idlock_exec.ctrl_switch_P1= 0x01;
      idlock_exec.ctrl_switch_C1R1= 0x02;
      idlock_exec.ctrl_switch_graceful_exit= 0x03;
      idlock_exec.ctrl_switch_forced_exit= 0x04;
      //--
      idlock_exec.response_notsupported= 0xff;
      idlock_exec.response_error= 0x00;
      idlock_exec.response_ok= 0x01;
      idlock_exec.response_wait= 0x02;
      idlock_exec.response_reserved= 0x03;
      idlock_exec.response_not_found= 0x04;
      //--
      idlock_exec.timer_no_delay= 0x01;
      idlock_exec.timer_delay= 0xff;
      //--
      idlock_exec.lost_ack_empty= 0x00;
      idlock_exec.lost_ack_mark= 0x01;
      //--
      idlock_exec.lockstat_invalid= 0x10;
      idlock_exec.lockstat_does_not_exist= 0x11;
      idlock_exec.lockstat_lvl1_in_progress= 0x12;
      idlock_exec.lockstat_lvl1_done= 0x13;
      idlock_exec.lockstat_lvl2_in_progress= 0x14;
      idlock_exec.lockstat_lvl2_stable= 0x15;
      idlock_exec.lockstat_lvl2_done= 0x16;
      //--
      idlock_exec.idxping_repeat0= 0x01;
      idlock_exec.idxping_repeat2= 0x02;
      idlock_exec.idxping_repeat3= 0x03;
      idlock_exec.idxping_warning1= 0x04;
      idlock_exec.idxping_warning2= 0x05;
      idlock_exec.idxping_fail1= 0x06;
      idlock_exec.idxping_fail2= 0x07;
      idlock_exec.idxping_fail9= 0x08;
      idlock_exec.idxping_fail20= 0x09;
      idlock_exec.idxping_invalid= 0x0a;
      //--
      idlock_exec.idx_pkt_len= System.Convert.ToUInt64(
          (win_com.node_count*24) +78);
      idlock_exec.lck_pkt_len= ((3*8) +70);
      //--
      return;}
    //--------
    private static System.UInt64[] string_to_integer_array(
          System.String string_input) {
      System.Int32 i1;
      System.UInt64[] ret;
      System.String[] s_a;
      //--
      if (string_input=="") throw new System.Exception(
          "idlock_exec.string_to_integer_array() empty input");
      //--
      s_a= string_input.Split('+');
      //--
      ret= new System.UInt64[s_a.Length];
      for(i1=0; i1< s_a.Length; i1++) {
        if (s_a[i1]=="") throw new System.Exception(
           "idlock_exec.string_to_integer_array() empty param");
          //System.Console.WriteLine("*"+s_a[i1]+"*");
        ret[i1]= System.Convert.ToUInt64(s_a[i1], 16);
        continue;}
      //--
      return ret;}
    //--------
    public static System.UInt64[] call_diag_general() {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_diag_general.ToString("x2");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if (ret.Length==6) return ret;
      throw new System.Exception("id-lock out params");}
    //--------
    public static System.UInt64[] call_config_1st(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_config_1st.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[1].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[2].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[3].ToString("x2");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.call_config_1st() #error");
        //if ((ret.Length==1) &&
        //    (ret[0]==idlock_exec.response_notsupported))
        //    throw new System.Exception(
        //    "idlock_exec.call_config_1st() #not supported");
      if (ret.Length==1) return ret;
      throw new System.Exception(
          "idlock_exec.call_config_1st() #output content");}
    //--------
    public static System.UInt64[] call_config_2nd(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_config_2nd.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[1].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[2].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[3].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[4].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[5].ToString("x4");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.call_config_2st() #error");
        //if ((ret.Length==1) &&
        //    (ret[0]==idlock_exec.response_notsupported))
        //    throw new System.Exception(
        //    "idlock_exec.call_config_2st() #not supported");
      if (ret.Length==1) return ret;
      throw new System.Exception(
          "idlock_exec.call_config_2st() #output content");}
    //--------
    public static System.UInt64[] call_ctrl_on_off(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_ctrl_on_off.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x2");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.call_ctrl_on_off() #error");
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_notsupported))
          throw new System.Exception(
          "idlock_exec.call_ctrl_on_off() #not supported");
      if (ret.Length==1) return ret;
      throw new System.Exception(
          "idlock_exec.call_ctrl_on_off() #output content");}
    //--------
    public static System.UInt64[] call_ctrl_query() {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_ctrl_query.ToString("x2");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.call_ctrl_query() #error");
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_notsupported))
          throw new System.Exception(
          "idlock_exec.call_ctrl_query() #not supported");
      if (ret.Length==1) return ret;
      if (ret.Length==7) return ret;
      throw new System.Exception(
          "idlock_exec.call_ctrl_query() #output content");}
    //--------
    public static System.UInt64[] call_idx_query(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_idx_query.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x2");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.call_idx_query() #error");
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_notsupported))
          throw new System.Exception(
          "idlock_exec.call_idx_query() #not supported");
      if (ret.Length==1) return ret;
      if (ret.Length==(4*win_com.node_count +1)) return ret;
      throw new System.Exception(
          "idlock_exec.call_idx_query() #output content");}
    //--------
    public static System.UInt64[] call_idx_set(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_idx_set.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[1].ToString("x16");
      s_xchg+= "+";
      s_xchg+= t_input[2].ToString("x16");
      s_xchg+= "+";
      s_xchg+= t_input[3].ToString("x2");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.call_idx_set() #error");
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_notsupported))
          throw new System.Exception(
          "idlock_exec.call_idx_set() #not supported");
      if (ret.Length==1) return ret;
      throw new System.Exception(
          "idlock_exec.call_idx_set() #output content");}
    //--------
    public static System.UInt64[] call_lock_set(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_lock_set.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x16");
      s_xchg+= "+";
      s_xchg+= t_input[1].ToString("x16");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.call_lock_set() #error");
        //if ((ret.Length==1) &&
        //    (ret[0]==idlock_exec.response_notsupported))
        //    throw new System.Exception(
        //    "idlock_exec.call_lock_set() #not supported");
      if (ret.Length==1) return ret;
      throw new System.Exception(
          "idlock_exec.call_lock_set() #output content");}
    //--------
    public static System.UInt64[] call_lock_query(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_lock_query.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x16");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.call_lock_query() #error");
        //if ((ret.Length==1) &&
        //    (ret[0]==idlock_exec.response_notsupported))
        //    throw new System.Exception(
        //    "idlock_exec.call_lock_query() #not supported");
      if (ret.Length==1) return ret;
      throw new System.Exception(
          "idlock_exec.call_lock_query() #output content");}
    //--------
    public static System.UInt64[] call_res_id_lookup(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_res_id_lookup.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x16");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if (ret.Length!=2) throw new System.Exception(
          "idlock_exec.call_res_id_lookup() #output content");
      if (ret[0]==idlock_exec.response_error)
          throw new System.Exception(
          "idlock_exec.call_res_id_lookup() #error");
      if (ret[0]==idlock_exec.response_notsupported)
          throw new System.Exception(
          "idlock_exec.call_res_id_lookup() #not supported");
      return ret;}
    //--------
    public static System.UInt64[] call_tr_id_lookup(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_tr_id_lookup.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x16");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if (ret.Length!=2) throw new System.Exception(
          "idlock_exec.call_tr_id_lookup() #output content");
      if (ret[0]==idlock_exec.response_error)
          throw new System.Exception(
          "idlock_exec.call_tr_id_lookup() #error");
      if (ret[0]==idlock_exec.response_notsupported)
          throw new System.Exception(
          "idlock_exec.call_tr_id_lookup() #not supported");
      return ret;}
    //--------
    public static System.UInt64[] debug_list(
        System.UInt64[] t_input) {
      System.String s_xchg;
      System.UInt64[] ret;
      //--
      s_xchg= "";
      s_xchg+= idlock_exec.cmd_debug_list.ToString("x2");
      s_xchg+= "+";
      s_xchg+= t_input[0].ToString("x16");
      //--
      s_xchg= win_com.test_cmd(s_xchg);
      ret= idlock_exec.string_to_integer_array(s_xchg);
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_error))
          throw new System.Exception(
          "idlock_exec.debug_list_output() #error");
        //if ((ret.Length==1) &&
        //    (ret[0]==idlock_exec.response_notsupported))
        //    throw new System.Exception(
        //    "idlock_exec.debug_list_output() #not supported");
      if ((ret.Length==1) &&
          (ret[0]==idlock_exec.response_notsupported))
          return ret;
      if (ret.Length==44) return ret;
      throw new System.Exception(
          "idlock_exec.debug_list_output() #output content");}
    //--------
    //--------
    }
  //--------
  }



