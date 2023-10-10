**``First draft``**

# The Linux application

I designed the ``id_lock`` to work on a cluster.
Each of your servers should start an instance and configure it before use.
Your servers should be able to send ``UDP`` packages to each other.

The ``id_lock`` runs on one ``CPU`` thread only.
Consider starting more than one ``id_lock`` cluster if you need more performance.
Of course, you can separate them only if they handle fully separated information.

## Starting

The ``id_lock`` starts with one parameter.
The parameter is a hexadecimal 16-bit port number.
The ``id_lock`` binds that ``UDP`` port on start.
Example with command port ``50433``:

``id-lock-0.0.2 c501``

## API

The ``id_lock`` uses the command port to communicate with applications.
Every valid request packet results in one output packet.
The ``id_lock`` sends the output message to the source network address of the request.
Use a different port for every communication thread.
There is no control over lost packages.
Use the ``API`` on localhost only.
Localhost communication does not lose packets.

## Message format

The input and output messages are lower-case text strings in the ``UDP`` packet.
The message contains hexadecimal numbers and separators between them.
The id_lock uses the ``+`` sign as a separator.

# Functions:

## 0x00 def_cmd_diag_general
* For diagnostic purposes only.

**req:**
* 00

**out:**
* com_in_lck_count+com_in_idx_count+com_in_err_count+com_out_lck_count+com_out_idx_count+event_flags
  * com_in_lck_count: _lock packets on input
  * com_in_idx_count: _idx packets on input
  * com_in_err_count: unresolved packets on input
  * com_out_lck_count: _lock packets sent out
  * com_out_idx_count: _idx packets sent out
  * event_flags: error event flags

## 0x01 def_cmd_config_1st
* The first configuration message is mandatory.

**req:**
* 01+c1+c2+c3+c4
  * c1: Server node count.
Limited to [3, 5, 7, 9] node count.
  * c2: Local node idx (1..)
  * c3: Communication repeat time, 0x02..0x3f * 20 millisec
  * c4: Parallel tasks on the cluster, 0x01..0x3f * 15 lock slots.
Limited to [3-250] tasks per node.

**out:**
* 01 (ok)
* 00/ff (error)

## 0x02 def_cmd_config_2nd
* Consecutive configuration messages are mandatory before other API messages.
* Every id_lock instance uses a UDP port for intercommunication.
Configuring them is mandatory for each valid node.

**req:**
* 02+node_idx+ip4+ip3+ip2+ip1+port
  * node_idx: Target node idx
  * ip4: Highest byte of the ipv4 address
  * ip3, ip2: Middle bytes of the ipv4 address
  * ip1: Lowest byte of the ipv4 address
  * port: The target node's intercommunication port. 
  * Example: ``02+01+7f+00+00+01+115c``

**out:**
* 01 (ok)
* 00/ff (error)

## 0x11 def_cmd_ctrl_on_off
* Check the header values in the tester's source code.

**req:**
* 11+command
  * command:
    * def_cmd_switch_P1: Z->P
    * def_cmd_switch_C1R1: P->C/R
    * def_cmd_switch_graceful_exit: N->Z
    * def_cmd_switch_forced_exit

**out:**
* 01 (ok)
* 00/ff (error)

## 0x12 def_cmd_ctrl_query
* Check the header values in the tester's source code.

**req:**
* 12

**out:**
* 00 (error)
* status+mainstat_aux+cluster_status+delay+mainloop_load_last+mainloop_load_avg+mainloop_load_max
  * status:
    * (Z)ero communication
    * (P)re-start
    * (C)old start
    * (R)ejoin
    * (N)ormal operation
  * mainstat_aux: Phase of the active status.
  * cluster_status: Additional status support for the protocol.
  * delay: Additional timing support for the protocol.
  * mainloop_load_*: ``CPU`` load %.
On 100%, the shutdown is automatic.

## 0x21 def_cmd_idx_query

**req:**
* 21+node_idx
  * node_idx: The target node.

**out:**
* 00/ff (error)
* com_status+(seq_0+seq_1+node_status+flags)[1]+...+flags[n]
  * com_status: Reaction delay from that node. Warning limit: 04
  * seq_0, seq_1: Shared data on the cluster from that node.
  * node_status: Known activity state (Z,P,C,R,N).
  * flags: For debug purposes only.

## 0x22 def_cmd_idx_set
* Check the header values in the tester's source code.

**req:**
* 22+node_idx+seq_0+seq_1+lost_ack
  * node_idx: Local node index.
  * seq_0: The largest continuous transaction_id that the local node shares from that source node.
  * seq_1: The largest continuous processed transaction_id on the local node from that source node.
  * lost_ack: Acknowledgement to the lost connection.

**out:**
* 01 (ok)
* 00/ff (error)

## 0x31 def_cmd_lock_set
* Check the header values in the tester's source code.
* Use def_cmd_lock_query to monitor the progress.

**req:**
* 31+resource_id+transaction_id
  * resource_id: Unique resource id on the cluster to be locked.
  * transaction_id: Unique transaction id on the cluster, member of a strict sequence

**out:**
* 00/ff (error)
* def_cmdresponse_ok
* def_cmdresponse_wait
* def_cmdresponse_reserved

## 0x32 def_cmd_lock_query
* Check the header values in the tester's source code.
* The transaction flow takes care of the eviction automatically.

**req:** 
* 32+transaction_id
  * transaction_id: *as above*

**out:**
* 00/ff (error)
* def_cmd_lockstat_does_not_exist
* def_cmd_lockstat_lvl1_in_progress
* def_cmd_lockstat_lvl1_done
* def_cmd_lockstat_lvl2_in_progress
* def_cmd_lockstat_lvl2_stable
* def_cmd_lockstat_lvl2_done
  * The resource locked safely on lvl2_done.

## 0x33 def_cmd_res_id_lookup
* Check the header values in the tester's source code.
* Please note that this function exists only to reduce the digital trash in the transaction flow.

**req:** 
* 33+resource_id
  * resource_id: *as above*

**out:** 
* status + transaction_id
  * status:
    * def_cmdresponse_error
    * def_cmdresponse_notsupported
    * def_cmdresponse_not_found
    * def_cmdresponse_ok
      * The transaction_id is valid.
  * transaction_id: The transaction id that owns the resource id.

## 0x34 def_cmd_tr_id_lookup
* Check the header values in the tester's source code.
* Please note that this function exists only to reduce the digital trash in the transaction flow.

**req:** 
* 34+transaction_id
  * transaction_id: *as above*

**out:** 
* status + resource_id
  * status:
    * def_cmdresponse_error
    * def_cmdresponse_notsupported
    * def_cmdresponse_not_found
    * def_cmdresponse_ok
      * The resource_id is valid.
  * resource_id: The resource id that belongs to the transaction id.

## 0x41 def_cmd_debug_list
* Does not work in the public version.

