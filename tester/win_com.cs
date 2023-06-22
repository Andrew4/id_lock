
namespace konzol {
  //--------
  public class win_com_ip {
    public System.String ip;
    public System.Int32 port;
    //--------
    public win_com_ip() {
      this.ip= "";
      this.port= -1;
      return;}
    //--------
    }
  //--------
  public static class win_com {
    //--------
    public static System.Int32 node_count;
    public static System.Int32 own_idx;
    //--
    private static System.String wsl_remote_ip;
    private static System.Net.Sockets.UdpClient udp_handle;
    private static System.Int32 portbase_win;
    private static System.Int32 portbase_cmd;
    private static win_com_ip[] port_cmd;
    //--------
    public static void init() {
      System.Int32 i1;
      //--
      win_com.node_count= 3; //3, 5, 7, 9
        //If you have a high-performance environment, it is ok to
        //  increase it and run more instances.
        //Please, read the warning at "idlock_xchg.jiffy_multiplier".
      win_com.own_idx= 0;
        //invalid idx means localhost-only autoconfig
      //--
      win_com.wsl_remote_ip= "172.30.95.231";
        //An Ubuntu guest OS tells you the IP if you ask:
        //  ip r
        //If you cannot ping that from the host, update the WSL:
        //  wsl --update
        //Also, install the latest kernel update from GitHub:
        //  https://github.com/microsoft/WSL2-Linux-Kernel/releases
      //--
      win_com.udp_handle= null;
      //--
        //Up to 15 cluster nodes, then offset it for more distance:
      win_com.portbase_win= 0xc520; //50464 + 1..
      win_com.portbase_cmd= 0xc500; //50432 + 1..
      //--
        //For the sake of the test I kept it simple.
      win_com.port_cmd= new win_com_ip[(win_com.node_count+1)];
      for(i1=1; i1< (win_com.node_count+1); i1++) {
        win_com.port_cmd[i1]= new win_com_ip();
        win_com.port_cmd[i1].ip= win_com.wsl_remote_ip;
        win_com.port_cmd[i1].port= win_com.portbase_cmd+ i1;
        continue;}
      //--
      return;}
    //--------
    public static System.Boolean udp_port_bind() {
      System.Int32 i1;
      System.UInt64 ull1;
      //--
      if ((win_com.own_idx< 1) ||
          (win_com.own_idx >win_com.node_count)) {
        //--
        win_com.own_idx= 0;
        for(i1=1; i1< (win_com.node_count+1); i1++) {
          try {
            win_com.udp_handle=
                new System.Net.Sockets.UdpClient(
                win_com.portbase_win + i1);
            win_com.own_idx= i1;} catch (System.Exception) { }
          if (win_com.own_idx >0) break;
          continue;}}
      if (win_com.own_idx==0) return false;
      //--
      ull1= System.Convert.ToUInt64(win_com.own_idx);
      ull1= ull1 << (7*8);
      idlock_xchg.next_transaction_id+= ull1;
      //--
      return true;}
    //--------
    public static void udp_port_close() {
      //--
      if (win_com.udp_handle!=null) {
        win_com.udp_handle.Close();
        win_com.udp_handle= null;}
      //--
      return;}
    //--------
    public static System.String test_cmd(System.String t_s) {
        //The id_lock must run in the background, or else this
        //  function is going to freeze the application.
      System.String t_ret;
      System.String r_ip, s_ip;
      System.Int32 r_port, s_port;
      System.Byte[] t_bytes;
      System.Net.IPEndPoint ip_endpoint;
      //--
      t_ret= "";
      //--
      while(true) {
        if (win_com.udp_handle.Available< 1) break;
        ip_endpoint= new System.Net.IPEndPoint(
            System.Net.IPAddress.Any, 0);
        t_bytes= win_com.udp_handle.Receive(ref ip_endpoint);
        continue;}
      //--
      r_ip= win_com.port_cmd[win_com.own_idx].ip;
      r_port= win_com.port_cmd[win_com.own_idx].port;
      t_bytes= System.Text.Encoding.ASCII.GetBytes(t_s);
        //System.Console.WriteLine(
        //    "win_com.test_cmd() is sending a message *"+
        //    t_s+ "* to "+ r_ip+ ":"+ r_port.ToString());
      win_com.udp_handle.Send(
          t_bytes, t_bytes.Length, r_ip, r_port);
      //--
      while(true) {
        ip_endpoint= new System.Net.IPEndPoint(
            System.Net.IPAddress.Any, 0);
        t_bytes= win_com.udp_handle.Receive(ref ip_endpoint);
        s_ip= ip_endpoint.Address.ToString();
        s_port= ip_endpoint.Port;
        if (s_ip!=r_ip) continue;
        if (s_port!=r_port) continue;
        t_ret= System.Text.Encoding.UTF8.GetString(t_bytes);
          //System.Console.WriteLine(
          //    "win_com.test_cmd() has received an answer *"+
          //    t_ret+"* from the same address");
        break;}
      //--
      return t_ret;}
    //--------
    //--------
    }
  //--------
  }

