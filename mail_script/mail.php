<?php

error_reporting(E_ERROR);
$strRootPath = 'mail';

require_once( $strRootPath ."/class/mail.class.php");
require_once( $strRootPath ."/class/mail_decode.class.php");

class Mail extends Controller {




	//echo $strRootPath ."pop3.class.php5.inc";
	// Socket Options

	function Mail()
	{
		parent::Controller();
	}
	function email($darid = '0', $adrid = '0', $uzid = '0', $box = '0', $mail = '0', $rodyti = '0')
	{
		$dar_data = $this->session->userdata('ud');
		$darid = $dar_data['darid'];
		
		if(empty($darid)) redirect(site_url($this->page->_LangCode.'/user/login_dar'), 'refresh');
		global $_GET;
		
//		Kadangi pagal sena mail.class.php viska ima is $_GET, viska pakeisti....:)

		$_GET['box'] = $box;
		$_GET['darid'] = $darid;
		$_GET['adrid'] = $adrid;
		$_GET['uzid'] = $uzid;
		$_GET['mail'] = $mail;
		$_GET['rodyti'] = $rodyti;
		
		$data['main_view'] = "mail/mail";
		$data['page_name'] = "Pašto dėžutė";
		
		
		$this->page->_Meniu['left'] = array(
		anchor($this->page->_LangCode."/mail/email/".$darid, 'Gauti laiškai'),
		anchor($this->page->_LangCode."/mail/email/".$darid."/".$adrid."/".$uzid."/siustieji", 'Išsiųsti laiškai'),
		anchor($this->page->_LangCode."/mail/email/".$darid."/".$adrid."/".$uzid."/juodrastis", 'Ruošiniai'),
		anchor($this->page->_LangCode."/mail/email/".$darid."/".$adrid."/".$uzid."/0/new", 'Naujas laiškas')
		);

		$data['html'] = $this->get_emails($darid, $adrid, $uzid, $box, $mail, $rodyti);
		$data['disp_search'] = 0;

		$this->load->view("page", $data);
	}

	function get_emails($darid = '0', $adrid = '0', $uzid = '0', $box = '0', $mail = '0', $rodyti = '0')
	{
		$objMails = new mail_vaizdas();

		if (!is_numeric($rodyti) and $rodyti == 'kontaktus')
		{
			$disp.= $objMails->displayKontaktai();
			return $disp;
		}
		/**
		 * Remember that the encryption support doesn't work at time for the socket extension
		 * This will I implement later.
		 * 
		 */
		$bUseSockets = FALSE;
		$bUseTLS = TRUE;
		$bIPv6 = FALSE;
		$arrConnectionTimeout = array( "sec" => 10,
		"usec" => 500 );


		$rez = $this->db->query("select reiksme, d_email from dar
inner join charak_iv_dat on charak_iv_dat.did = dar.darid
inner join charak_iv_zod on charak_iv_zod.cizid = charak_iv_dat.cizid and charak_iv_zod.papild='email_pass'
where darid='$darid'")->row();

		$vardas = current(explode("@",$rez->d_email));

        var_dump($vardas);

		// POP3 Options
		$strProtocol= "tls";
		$strHost = "smtp.gmail.com";
		$intPort = 995;
		$strUser = $vardas;
		$strPass = $rez->reiksme;
		$bAPopAutoDetect = TRUE;
		$bHideUsernameAtLog = FALSE;

		// Logging Options
		$strLogFile = "php://stdout";//$strRootPath. "pop3.log";

		// EMail store Sptions
		$strPathToDir = $strRootPath."mails" .DIRECTORY_SEPARATOR;
		$strFileEndings = ".eml";


		try
		{
			// Instance the POP3 object
			$objPOP3 = new POP3( $strLogFile, $bAPopAutoDetect, $bHideUsernameAtLog, $strProtocol, $bUseSockets );

			// Connect to the POP3 server
			$objPOP3->connect($strHost,$intPort,$arrConnectionTimeout,$bIPv6);

			// Logging in
			$objPOP3->login($strUser, $strPass);

			// Get the office status
			$arrOfficeStatus = $objPOP3->getOfficeStatus();

			/**
     * This for loop store the messages under their message number on the server
     * and mark the message as delete on the server.
     */
			//"Hello world!";
			for($intMsgNum = 1; $intMsgNum <= $arrOfficeStatus["count"]; $intMsgNum++ )
			{
				$EML1=$objPOP3->getMsg($intMsgNum);

				$mail_obj = new MIMEDECODE($EML1, "\r\n");
				$mail = $mail_obj->get_parsed_message();
				$i++;


				$objPOP3->insertToDB($mail);
				//$objPOP3->saveToFileFromServer($intMsgNum, $strPathToDir, $strFileEndings);
				//        $objPOP3->deleteMsg($intMsgNum);

			}

			$disp = "<div class='td-thead-l'>Gauta naujų laiškų: <b>{$arrOfficeStatus['count']}</b></div></br>";



			// Send the quit command and all as delete marked message will remove from the server.
			// IMPORTANT:
			// If you deleted many mails it could be that the +OK response will take some time.
			$objPOP3->quit();

			// Disconnect from the server
			// !!! CAUTION !!!
			// - this function does not send the QUIT command to the server
			//   so all as delete marked message will NOT delete
			//   To delete the mails from the server you have to send the quit command themself before disconnecting from the server
			$objPOP3->disconnect();

			//atvaizduojam laiskus
			$disp.= "
   <script>
   function rodyti_slepti(element){
	
		var id = element;
		var eilute=document.getElementById('eilute'+id);
		
       var e = document.getElementById(id);
       if(e.style.display == 'table-row')
          {e.style.display = 'none';
          eilute.setAttribute('bgcolor','Silver');}
       else
          {e.style.display = 'table-row';
		 eilute.setAttribute('bgcolor','Gainsboro');}
}
	</script>
   ";

			if ($mail=='new')
			{
				$disp.= $objMails->displayNewMail();
				if(!empty($_POST['send'])) $disp.=$objMails->sendMail($strUser, $strPass, $rez->d_email);
				if(!empty($_POST['save'])) $disp.=$objMails->saveMail($strUser, $strPass, $rez->d_email);
				if(!empty($_FILES['file'])) $disp.=$objMails->upload_file();
			}
			elseif ($uzid>0)
			{
				$visi_laiskai = $objMails->uzid_adrid();
				$arr = $objMails->uz_mail($visi_laiskai);
				$disp.= $objMails->displayUzMail($arr);
			}
			else
			{
				if($box=='siustieji') $arr = $objMails->getMails(null,$vardas.'@gmail.com');
				elseif($box=='juodrastis') $arr = $objMails->getMails(null,null,'1');
				else $arr = $objMails->getMails($vardas.'@gmail.com',null);
				$disp.= $objMails->displayWebMail($arr);
			}

			$disp.= '
	<script type="text/javascript">
	$(function() {
		$( ".datepicker" ).datepicker();
	});
	</script>
	</body></html>';
		}
		catch( POP3_Exception $e )
		{
			die($e);
		}
		return $disp;
	}
}
// Your next code

?> 
