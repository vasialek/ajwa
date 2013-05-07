<?php
/**
 * POP3 Class
 * 
 * This class provide access to a pop3 server through the pop3 protocol
 *  
 * @need: >=php-5.2.x
 * @author: j0inty.sL
 * @email: bestmischmaker@web.de
 * @version: 0.7.2-beta
 *
 * NOTES:
 * - IPv6 support NEVER tested at time
 */

@mysql_connect('localhost','root','123456') or die ("Negalima prisijungti prie SQL serverio");
@mysql_select_db('turas') or Klaida_Conn('turas');

final class POP3_Exception extends Exception
{
	/**
	 * @param string $strErrMessage
	 * @param integer $intErrCode
	 * 
	 * @return POP3_Exception
	 */
	function __construct( $strErrMessage, $intErrCode )
	{
		switch( $intErrCode )
		{
			case POP3::ERR_NOT_IMPLEMENTS:
				if( empty($strErrMessage) ) $strErrMessage = "This function isn't implements at time.";
				break;

			case POP3::ERR_SOCKETS:
				$strErrMessage = "Sockets Error: (". socket_last_error() .") -- ". socket_strerror(socket_last_error());
				break;

			case POP3::ERR_STREAM:
			case POP3::ERR_LOG:
				$aError = error_get_last();
				$strErrMessage = "Stream Error: (". $aError["type"] .") -- ". $aError["message"];
				break;
		}
		parent::__construct($strErrMessage, $intErrCode);
	}


	/**
	 * Store the Exception string to a given file
	 * 
	 * @param string $strLogFile  logfile name with path
	 */
	public function saveToFile($strLogFile)
	{
		if( !$resFp = @fopen($strLogFile,"a+") )
		{
			return false;
		}
		$strMsg = date("Y-m-d H:i:s -- ") . $this;
		if( !@fputs($resFp, $strMsg, strlen($strMsg)) )
		{
			return false;
		}
		@fclose($resFp);
	}
	/**
	 * @return string Exception with StackTrace as String
	 */
	public function __toString()
	{
		return __CLASS__ ." [". $this->getCode() ."] -- ". $this->getMessage() ." in file ". $this->getFile() ." at line ". $this->getLine(). PHP_EOL ."Trace: ". $this->getTraceAsString() .PHP_EOL;
	}

}
class mail_vaizdas{

	var $_conv_from = 'utf-8';
	var $_conv_to = 'utf-8';

	function uzid_adrid(){
		if (strpos($_SERVER[REQUEST_URI],'&visi_laiskai')){
			$visi_laiskai='taip';
			$href=str_replace('&visi_laiskai','',$_SERVER[REQUEST_URI]);
			$disp.=  iconv($this->_conv_from,$this->_conv_to,"<a href='$href'>Užsakymo laikotarpio laiškai</a> &nbsp;&nbsp;<b>Visi užsakovo laiškai</b></br>");
		}
		else{
			$visi_laiskai='ne';
			$href=$_SERVER[REQUEST_URI]."&visi_laiskai";
			$disp.=  iconv($this->_conv_from,$this->_conv_to,"<b>Užsakymo laikotarpio laiškai</b> &nbsp;&nbsp;<a href='$href'>Visi užsakovo laiškai</a></br>");
		}
		return $disp;
	}


	function uz_mail($visi_laiskai){
		$darid=explode("_",$_GET["darid"]);
		$darid=$darid['0'];
		$uzid=$_GET['uzid'];
		$adrid=$_GET['adrid'];
		//jei yra uzid tai pagal ji, jei adrid tai pagal sita
		$sql="select uzsakovai.at_e,uzsakovai.username,dar.d_email,uz.uzdat,scn.scndat from uz
inner join uzsakovai on uzsakovai.adrid = uz.adrid_g
inner join dar on dar.darid='$darid'
left join scn on scn.scnipag=uz.uzid
where uzid='$uzid' or adrid_g='$adrid' limit 1";//print $sql;
		$RS=@mysql_query($sql);
		for($i=1;$i<=@mysql_num_rows($RS);$i++){
			$rr=@mysql_fetch_array($RS);
			$dar_adr=$rr["d_email"];
			//	$dar_adr='ricardas.meskauskas@gmail.com';
			$uz_adr=$rr["at_e"];
			//	$uz_adr='ricardas@ignet.lt';
			$uzdat=$rr["uzdat"];
			$scndat=$rr["scndat"];
			$username=$rr["username"];
			if(empty($scndat))$scndat="2999-12-31";
		}
		if(!empty($uzid) and $visi_laiskai!='taip')
		$cond_laikas=" and data>='$uzdat' and data<='$scndat' ";
		$sql="select * from uzsakovai_inbox where ((siunt_adresas='$uz_adr' and adresatas='$dar_adr') or (siunt_adresas='$dar_adr' and adresatas='$uz_adr')) $cond_laikas order by data desc";//print $sql;
		//print $sql;
		$RS=@mysql_query($sql);
		for($i=1;$i<=@mysql_num_rows($RS);$i++){
			$rr=@mysql_fetch_array($RS);
			$arr[]=array($rr);
		}//print_r ($arr[0][0]);
		$arr['username']=$username;
		return $arr;
	}


	function displayUzMail($arr=array()){
		$disp.= "<table border=0 cellpadding='2'>
				<tr>
					<td class='td-thead-l'><b>Klientas</b>
					</td>
					<td class='td-thead-r'><b>Siuntejas</b>
					</td>
					<td class='td-thead-r'><b>Tema</b>
					</td>
					<td class='td-thead-r'><b>Data</b>
					</td>
				</tr>";

		foreach ($arr as $key => $reiksme)	{
			$disp.= "<tr style='cursor: pointer;' onclick='rodyti_slepti(&quot;{$reiksme['0']['id']}&quot;)'>
				<td class='td-tbody-sl'>
					".iconv($this->_conv_from,$this->_conv_to,$arr['username'])."
				</td>
				<td class='td-tbody-sr'>
					{$reiksme['0']['siunt_adresas']}
				</td>
				<td class='td-tbody-sr'>
					".iconv($this->_conv_from,$this->_conv_to,$reiksme['0']['tema'])."
				</td>
				<td class='td-tbody-sr'>
					{$reiksme['0']['data']}
				</td>
			</tr>
			<tr id={$reiksme['0']['id']} style='display:none'>
				<div>
				<td colspan='4' class='td-tbody-sr'>
		";
			if (!empty($reiksme['0']['html_text']))
			$tekstas=$reiksme['0']['html_text'];
			else
			$tekstas=$reiksme['0']['plain_text'];
			$disp.= "
					".iconv($this->_conv_from,$this->_conv_to,$tekstas)."
				</br>			
					{$reiksme['0']['papild']}
				</td>
				</div>
			</tr>
		";
		}
		$disp.= "</table>";

		return $disp;
	}
	function displayWebMail($arr=array()){

		global $_GET;

		$disp.=  "<table border='0' width='100%'>
				<tr>
					<td class='td-thead-l' width='160px'>".iconv($this->_conv_from,$this->_conv_to,'Pašto dėžutė')."</td>
					<td>
						<form method='POST'>
						<div  class='td-thead-l'>
						<span>Data nuo: <input type='text' name='data_nuo' value='{$_POST['data_nuo']}' class='datepicker'></span>
						<span>Data iki: <input type='text' name='data_iki' value='{$_POST['data_iki']}' class='datepicker'></span>
						<span>Klientas: <input type='text' name='klientas' value='{$_POST['klientas']}'></span>
						<span>".iconv($this->_conv_from,$this->_conv_to,'Pagal raktinį žodį').": <input type='text' name='text' value='{$_POST['text']}'></span>
						
						<input type='hidden' name='darid' value='{$_GET['darid']}'>
						<input type='hidden' name='uzid' value='{$_GET['uzid']}'>
						<input type='hidden' name='box' value='{$_GET['box']}'><input type='submit' 
						value='".iconv($this->_conv_from,$this->_conv_to,'Ieškoti')."'>
						</div>
						</form>
					</td>
				</tr>
				<!--tr>
					<td valign='top'>
						<table>
							<tr>
								<td class='td-thead-l'>";
		if ($_GET['mail']=='new')
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<b>Rašyti laišką</b>");
		else
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<a href='?darid=".$_GET['darid']."&adrid=".$_GET['adrid']."&uzid=".$_GET['uzid']."&mail=new'>Rašyti laišką</a>");
		$disp.= "	</td>
							</tr>
							<tr>
								<td>
									</br>
								</td>
							</tr>
							<tr>
								<td class='td-thead-l'>";

		if ($_GET['box']=='gautieji' or ($_GET['box']<>'siustieji' and $_GET['box']<>'juodrastis'))
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<b><li>Gauti laiškai</li></b>");
		else
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<a href='?darid=".$_GET['darid']."&adrid=".$_GET['adrid']."&uzid=".$_GET['uzid']."&box=gautieji'><li>Gauti laiškai</li></a>");
		$disp.= "	</td>
							</tr>
							<tr>
								<td class='td-thead-l'>";
		if ($_GET['box']=='siustieji')
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<b><li>Išsiųsti laiškai</li></b>");
		else
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<a href='?darid=".$_GET['darid']."&adrid=".$_GET['adrid']."&uzid=".$_GET['uzid']."&box=siustieji'><li>Išsiųsti laiškai</li></a>");
		$disp.= "	</td>
							</tr>
							<tr>
								<td class='td-thead-l'>";
		if ($_GET['box']=='juodrastis')
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<b><li>Ruošiniai</li></b>");
		else
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<a href='?darid=".$_GET['darid']."&adrid=".$_GET['adrid']."&uzid=".$_GET['uzid']."&box=juodrastis'><li>Ruošiniai</li></a>");
		$disp.= "</td>
							</tr-->
						</table>
					</td>
					<td valign='top'>
						<table width='100%' cellpadding=0 cellspacing=0>
							<tr>
								<td class='td-thead-l'><b>#</b></td>
								<td class='td-thead-r'><b>Klientas</b></td>
								<td class='td-thead-r'><b>Siuntejas</b>
								</td>
								<td class='td-thead-r'><b>Tema</b>
								</td>
								<td class='td-thead-r'><b>Data</b>
								</td>
								<td class='td-thead-r'><b>Užsakymas</b>
								</td>
								<td class='td-thead-r'><b>Atsakyti</b>
								</td>
							</tr>								
					";
		foreach ($arr as $key => $reiksme)
		{
			if (!empty($reiksme['html_text'])) $tekstas=$reiksme['html_text'];
			else $tekstas=$reiksme['plain_text'];

			$disp.= "
							<tr style='cursor: pointer;' onclick='rodyti_slepti(&quot;{$reiksme['id']}&quot;);' id='eilute{$reiksme['id']}'>
							<form action='/index.php/lt/mail/email/".$_GET['darid']."/0/0/0/new' method='POST' name='atsakyti'>		
								<td class='td-tbody-sl'>";								
			if(!empty($reiksme['papild']))$disp.=  '#';else $disp.= "&emsp;";
			$disp.= "</td>
							<td class='td-tbody-sr'>
							";
			if (!empty($reiksme['at_e'])){ $vardas=$reiksme['username'];
			$uzsakymas='<a href=\'#uzsakymu_sarasas\'>Žiūrėti laišką</a>';}
			else {$vardas='<font color=\'red\'><b>!</b></font> '.$reiksme['siunt_vardas'];
			$uzsakymas='<a href="/lt/user/register/f/'.$_GET['darid'].'/dar/'.base64_encode($reiksme['siunt_adresas']).'" target="_blank">Naujas klientas</a>';}

			$disp.= 		iconv($this->_conv_from,$this->_conv_to,$vardas)."
								</td>
								<td class='td-tbody-sr'>
									{$reiksme['siunt_adresas']}
									<div style='display:none'><input type='text' name='gavejas' value='".$reiksme['siunt_adresas']."'/></div>
								</td>
								<td class='td-tbody-sr'>
									".iconv($this->_conv_from,$this->_conv_to,$reiksme['tema'])."
									<div style='display:none'><input type='text' name='tema' value='Re: ".iconv($this->_conv_from,$this->_conv_to,$reiksme['tema'])."'/></div>
								</td>
								<td class='td-tbody-sr'>
									{$reiksme['data']}
								</td>
								<td class='td-tbody-sr'>
									".iconv($this->_conv_from,$this->_conv_to,$uzsakymas)."
								</td>
								<td>
								<div style='display:none'><input type='text' name='textboxas' value='".iconv($this->_conv_from,$this->_conv_to,$tekstas).
			$reiksme['papild']."'/></div>
								<input type='submit' name='answer' value='Atsakyti'/></td>
								</form>
							</tr>
							<tr id={$reiksme['id']} style='display:none'>
								<div>
									<td colspan='5'class='td-tbody-sr-p'>
						";

			$disp.= "
							".iconv($this->_conv_from,$this->_conv_to,$tekstas)."
							</br>			
							{$reiksme['papild']}
									</td>
								</div>
							</tr>
						";
		}
		$disp.= "</table>";

		return $disp;
	}
	function displayNewMail(){

		global $_GET;
		$disp.= "
			<table border='0' width='100%'>
				<tr>
					<td class='td-thead-l' width='160px'>".iconv($this->_conv_from,$this->_conv_to,'Pašto dėžutė')."</td>
					<td>
						<form id='rasymo_forma' method='POST'>
						<table  class='td-thead-l'>
							<tr>
								<td>Kam: </td>
								<td><input autocomplete='off' type='text' name='gavejas' id='gavejas' value='{$_POST['gavejas']}' size='100%' onkeyup=\"xmlhttpPost('mail.php?rodyti=kontaktus', 'rasymo_forma', 'kontaktu_sarasas', 'Palaukite...');document.getElementById('kontaktu_sarasas').style.display='block'\";></td>
							</tr>
							<tr>
							<td>
							</td>
							<td>
								<div id='kontaktu_sarasas' class='td-tbody-sr' style='display:none;position: absolute;'
								onblur=\"this.style.display='none'\" onclick=\"this.style.display='none'\" tabindex=0>
									.slkjsdf.jksdn.sdj
									dflkfdljkhfg
									sddfs
									dfj
								</div>
							<td>
							</tr>
							<tr>
								<td>Tema: </td>
								<td><input type='text' name='tema' value='{$_POST['tema']}' size='100%'></td>
							</tr>						
							<tr>
								<td></td>
								<td>
						
						<input type='hidden' name='darid' value='{$_GET['darid']}'>
						<input type='hidden' name='uzid' value='{$_GET['uzid']}'>
						<input type='hidden' name='box' value='{$_GET['box']}'>		
						<input type='hidden' name='atach' value='".$_FILES["file"]["name"]."'>				
								</td>
							</tr>
						</table>
						
						<input type='submit' name='send' value='"; 
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"Siųsti")."'>
						<input type='submit' name='save' value='";
		$disp.=  iconv($this->_conv_from,$this->_conv_to,"Išaugoti")."'>
					</td>
				</tr>
				<tr>
					<td valign='top'>
						<!--table>
							<tr>
								<td>
									</br>
								</td>
							</tr>
							<tr>
								<td class='td-thead-l'>";

		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<a href='?darid=".$_GET['darid']."&adrid=".$_GET['adrid']."&uzid=".$_GET['uzid']."&box=gautieji'><li>Gauti laiškai</li></a>");
		$disp.= "	</td>
							</tr>
							<tr>
								<td class='td-thead-l'>";

		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<a href='?darid=".$_GET['darid']."&adrid=".$_GET['adrid']."&uzid=".$_GET['uzid']."&box=siustieji'><li>Išsiųsti laiškai</li></a>");
		$disp.= "	</td>
							</tr>
							<tr>
								<td class='td-thead-l'>";

		$disp.=  iconv($this->_conv_from,$this->_conv_to,"<a href='?darid=".$_GET['darid']."&adrid=".$_GET['adrid']."&uzid=".$_GET['uzid']."&box=juodrastis'><li>Ruošiniai</li></a>");
		$disp.= "</td>
							</tr>
						</table-->
					</td>
					<td valign='top'>
						<table width='95%' cellpadding=0 cellspacing=0>
							<tr>
								<td class='td-thead-l'>
								<textarea name='textboxas' style='width:100%;height:480px'>{$_POST['textboxas']}</textarea>
								</td>
								</form>
							</tr>	
							<tr>
								<td>
								<form id='upload_form' enctype='multipart/form-data' method='post'>
						<input id='file' type='file' value='Pasirinkti...' size='32' name='file'>
						<input type='submit' value='".iconv($this->_conv_from,$this->_conv_to,'Pridėti failą')."'>
								</form>
								</td>
							</tr>							
					</table>";
		//		$this->sendMail();

		return $disp;
	}
	function displayKontaktai(){
		$arr= $this->getKontaktai();
		foreach ($arr as $key => $reiksme)
		{	if(!empty($reiksme['email'])){
			$disp.=  '<div class="td-tbody-sl-p" onclick="document.getElementById(\'gavejas\').value=\''.$reiksme['email'].'\'"
								onmouseover="this.setAttribute(\'class\', \'td-tbody-sl-darbupogrupiai\');"
								onmouseout="this.setAttribute(\'class\', \'td-tbody-sl-p\');"
								>'.$reiksme['adresatas'].' ';
			if(!empty($reiksme['imone']) and $reiksme['adresatas']<>$reiksme['imone'])
			$disp.= '&bdquo;'.$reiksme['imone'].'&rdquo; ';
			$disp.= '&lt;'.$reiksme['email'].'&gt;</div>';
		}
		}
		return $disp;
	}
	function getMails($gavejas=null,$siuntejas=null,$saved='NULL'){

		global $_GET;
		$cond = ($saved != 'NULL') ? ' saved = '.$saved.' ' : 'saved is NULL ';

		
		if($gavejas!=null)$cond.=" and  adresatas='{$gavejas}' ";
		elseif($siuntejas!=null)$cond.=" and siunt_adresas='{$siuntejas}' ";

		if(!empty($_POST['data_nuo'])) $cond.=" and data>='{$_POST['data_nuo']}' ";
		if(!empty($_POST['data_iki'])) $cond.=" and data<='{$_POST['data_iki']}' ";
		if(!empty($_POST['klientas'])) $cond.=" and username like '%{$_POST['klientas']}%' ";
		if(!empty($_GET['adrid'])) $cond.=" and adrid = {$_GET['adrid']} ";
		if(!empty($_POST['text'])) $cond.=" and (tema like '%{$_POST['text']}%' or html_text like '%{$_POST['text']}%' or plain_text like '%{$_POST['text']}%') ";
		if(!empty($_GET['uzid']))
		{
			$sql = "select uzdat, (select uzdat from uz where uzid = ".$_GET['uzid'].") as nowuzdat from uz
			where uzid<".$_GET['uzid']." and uzpoz != 'Z' and adrid_g = (select adrid_g from uz where uzid = ".$_GET['uzid'].") 
			order by uzid desc limit 1";
			$RS=@mysql_query($sql);
			$rr=@mysql_fetch_array($RS);
			$cond.=" and substr(uzsakovai_inbox.data,1,10)>'".$rr['uzdat']."' and substr(uzsakovai_inbox.data,1,10) <= '".$rr['nowuzdat']."'";
		}



		$sql="SELECT uzsakovai_inbox.id, adresatas, siunt_adresas,siunt_vardas, tema, data, html_text, plain_text, papild, adrid, username, at_e
		FROM uzsakovai_inbox
		left join uzsakovai on (uzsakovai.at_e=uzsakovai_inbox.siunt_adresas or uzsakovai.at_e=uzsakovai_inbox.adresatas)
		WHERE $cond 
		ORDER BY DATA desc";

		$RS=@mysql_query($sql);
		for($i=1;$i<=@mysql_num_rows($RS);$i++){
			$rr=@mysql_fetch_assoc($RS);
			$arr[]=array(
			'id'=>$rr['id'],
			'adresatas'=>$rr['adresatas'],
			'siunt_adresas'=>$rr['siunt_adresas'],
			'siunt_vardas'=>$rr['siunt_vardas'],
			'tema'=>$rr['tema'],
			'data'=>$rr['data'],
			'plain_text'=>$rr['plain_text'],
			'html_text'=>$rr['html_text'],
			'papild'=>$rr['papild'],
			'adrid'=>$rr['adrid'],
			'username'=>$rr['username'],
			'at_e'=>$rr['at_e'],
			);
		}
		return $arr;
	}
	function sendMail($user,$pass,$e_mail){

		require_once "mail/includes/Mail.php";
		require_once "mail/includes/Mail/mime.php";


		$from = '<'.$e_mail.'>';
		$to = $_POST['gavejas'];
		$subject = iconv('UTF-8','windows-1257',$_POST['tema']);
		$body = iconv('UTF-8','windows-1257',$_POST['textboxas']);
		$crlf = "\n";
//		die(dirname(__FILE__).'rrr');
		$file=str_replace('class','',dirname(__FILE__)).'files/'.$_POST['atach'];

		$host = "ssl://smtp.gmail.com";
		$port = "465";
		$username = $user;
		$password = $pass;

		$headers = array ('From' => $from,
		'To' => $to,
		'Subject' => $subject);


		$mime = new Mail_mime();


		$mime->setTXTBody($body);

		$mime->addAttachment($file);
		$body = $mime->get($mimeparams);

		$headers = $mime->headers($headers);

		$smtp = Mail2::factory('smtp',
		array ('host' => $host,
		'port' => $port,
		'auth' => true,
		'username' => $username,
		'password' => $password)
		);


		$mail = Mail2::send($to,$headers, $body);

		if (PEAR::isError($mail)) {
			$disp.= ("<p>" . $mail->getMessage() . "</p>");
		} else {
			$disp.= ("<p>".iconv($this->_conv_from,$this->_conv_to,'Pranešimas išsiųstas!')."</p>");
		}

		$RS=mysql_query("insert into uzsakovai_inbox set
			adresatas = '{$to}',
			siunt_vardas = '{$e_mail}', 
			siunt_adresas = '{$e_mail}',
			tema = '{$subject}',
			data = NOW(),
			plain_text = '{$body}'
			");
		return $disp;

	}
	function saveMail($user,$pass,$e_mail){

		require_once "mail/includes/Mail.php";
		require_once "mail/includes/Mail/mime.php";


		$from = '<'.$e_mail.'>';
		$to = $_POST['gavejas'];
		$subject = iconv('UTF-8','windows-1257',$_POST['tema']);
		$body = iconv('UTF-8','windows-1257',$_POST['textboxas']);
		$crlf = "\n";
		$file='/mail/files/'.$_POST['atach'];



		if(mysql_query("insert into uzsakovai_inbox set
			adresatas = '{$to}',
			siunt_vardas = '{$e_mail}', 
			siunt_adresas = '{$e_mail}',
			tema = '{$subject}',
			data = NOW(),
			plain_text = '{$body}',
			saved = '1'
			"))
		{
			$disp.= ("<p>".iconv($this->_conv_from,$this->_conv_to,'Pranešimas išsaugotas!')."</p>");
		}


		return $disp;

	}

	function upload_file()
	{

		//print_r($_FILES);
		if ($_FILES["file"]["error"] > 0)
		{
			print_r( "Klaida: " . $_FILES["file"]["error"] . "<br />");die;
		}
		else
		{
			// print_r ("Upload: " . $_FILES["file"]["name"] . "<br />");
			// print_r ("Type: " . $_FILES["file"]["type"] . "<br />");
			// print_r ("Size: " . ($_FILES["file"]["size"] / 1024) . " Kb<br />");
			//  print_r ("Temp file: " . $_FILES["file"]["tmp_name"] . "<br />");

			if (file_exists("/mail/files/" . $_FILES["file"]["name"]))
			{
				while(file_exists("/mail/files/" . $_FILES["file"]["name"]))
				{
					list($pradzia, $galune) = explode(".",$_FILES["file"]["name"],2);
					$_FILES["file"]["name"]=$pradzia.'-'.rand(1, 9999).'.'.$galune;
				}
				// print_r ($_FILES["file"]["name"] . " already exists. ");
			}



			if(move_uploaded_file($_FILES["file"]["tmp_name"],	str_replace('class','',dirname(__FILE__))."files/" . $_FILES["file"]["name"]))
			{

				//   print_r ("Stored in: " . "upload/" . $_FILES["file"]["name"]);
				$file_patch="/mail/files/" . $_FILES["file"]["name"];
				$disp.=  "<a target='_blank' href='/mail/files/".$_FILES["file"]["name"]."'>".$_FILES["file"]["name"]."</a>";
				$today = getdate();
				$today = $today[year].'-'.$today[mon].'-'.$today[mday];
				$pavadinimas=$_POST['dok_list'];
				if($rusis!='6')
				$uzid = $_SESSION['uzid'];
				$sql="INSERT into dok_list (rusis, id, dtip, pavad, failas, pdata) values ('$rusis','$uzid', '1', '$pavadinimas','$file_patch','$today')";
				$result=mysql_query($sql);
			}
		}
		return $disp;


	}
	function getKontaktai(){
		$like=$_POST['gavejas'];
		$sql="select at_e,concat(at_v,' ',at_p) as vardas,username as imone from uzsakovai
where at_e like '%$like%' or at_v like '%$like%' or at_p like '%$like%' or username like '%$like%'
union
select adrmail,concat(adrvar,' ',adrpav), adrorg from refadr
where adrmail like '%$like%' or adrvar like '%$like%' or adrpav like '%$like%' or adrorg like '%$like%'
union
select d_email,concat(d_var,' ',d_pav), pnam from dar
left join pad on pad.paid=dar.paid
left join pad_zodynas on pad.paid_v=pad_zodynas.padzid
where d_email like '%$like%' or d_var like '%$like%' or d_pav like '%$like%' or pnam like '%$like%'";
		$RS=@mysql_query($sql);
		for($i=1;$i<=@mysql_num_rows($RS);$i++){
			$rr=@mysql_fetch_array($RS);
			$arr[]=array(
			'email'=>$rr['at_e'],
			'adresatas'=>$rr['vardas'],
			'imone'=>$rr['imone'],
			);
		}
		return $arr;
	}
}
class POP3
{
	//const ERR_NONE = 0;
	const ERR_LOG = 1;
	const ERR_SOCKETS = 2;
	const ERR_PARAMETER = 3;
	const ERR_NOT_IMPLEMENTS = 4;
	const ERR_INVALID_STATE = 5;
	const ERR_STREAM = 6;
	const ERR_SEND_CMD = 7;


	const STATE_DISCONNECT = 100;
	const STATE_AUTHORIZATION = 101;
	const STATE_TRANSACTION = 102;

	/*
	const PROTOCOL_TCP = 1;
	const PROTOCOL_TLS = 2;
	const PROTOCOL_SSL = 3;
	const PROTOCOL_SSLV2 = 4;
	const PROTOCOL_SSLV3 = 5;
	*/
	const DEFAULT_BUFFER_SIZE = 4096;
	private $bLogOpened = FALSE;
	private $resLogFp = FALSE;
	private $strLogFile = NULL;
	private $bHideUsernameAtLog = TRUE;

	private $bUseSockets;
	private $strProtocol = NULL;
	private $bSocketConnected = FALSE;
	private $strHostname = NULL;
	private $strIPAdress = NULL;
	private $intPort = NULL;

	private $intCurState = self::STATE_DISCONNECT;
	private $strAPOPBanner = NULL;
	private $bAPOPAutoDetect;

	private $strVersion = "0.7.2-beta";


	/*
	* Constructor
	*
	* @param NULL|string $strLogFile  Path to a log file or NULL for no log
	* @param bool $bAPOPAutoDetect  APOP Auto Dection on|off
	* @param bool $bHideUsernameAtLog  Does the Username should hide at the log file
	* @param $strEncryption (tcp|ssl|sslv2|sslv3|tls) [depend on your PHP configuration]
	* @param bool $bUseSockets  Use the socket extension (default = TRUE) But it check is the extension_loaded, too
	*             !!! Only needed by them, who have the sockets extension loaded, but want use the stream functions !!!
	*
	* @throw POP3_Exception
	*/
	public function __construct( $strLogFile = NULL, $bAPOPAutoDetect = TRUE, $bHideUsernameAtLog = TRUE, $strEncryption = TRUE, $bUseSockets = TRUE )
	{
		if( !is_bool($bAPOPAutoDetect) )
		{
			throw new POP3_Exception("Invalid APOP auto detect parameter given.", self::ERR_PARAMETER);
		}
		if( !is_bool($bHideUsernameAtLog) )
		{
			throw new POP3_Exception("Invalid Hide Username at log file parameter given.", self::ERR_PARAMETER);
		}

		if( !preg_match("/^(tcp|ssl|sslv2|sslv3|tls)+$/", $strEncryption) )
		{
			throw new POP3_Exception("Invalid encryption parameter given. (tcp|ssl|sslv2|sslv3|tls) [depend on your PHP configuration]", self::ERR_PARAMETER);
		}
		else if( $bUseSockets && preg_match("/^(ssl|sslv2|sslv3|tls)+$/", $strEncryption))
		{
			throw new POP3_Exception("Encryption with Sockets Extension is not implemented now. Use \$UseSocket=false for that.",self::ERR_NOT_IMPLEMENTS );
		}

		// Activate logging if needed
		if( !is_null($strLogFile) )
		{
			$this->strLogFile = $strLogFile;
			$this->openlog();
		}
		// Check for sockets extension if needed
		if( $bUseSockets && extension_loaded("sockets") )
		{
			$this->bUseSockets = TRUE;
		}
		else
		{
			if( $bUseSockets )
			{
				$this->log("You choose to use the socket extensions support but this isn't available.");
			}
			$this->bUseSockets = FALSE;
		}
		// Activate or Deactivate APOP Auto Detect mechanism
		$this->bAPOPAutoDetect = $bAPOPAutoDetect;
		$this->bHideUsernameAtLog = $bHideUsernameAtLog;
		$this->strProtocol = $strEncryption;
	}
	/*
	* Destructor
	*
	* @throw POP3_Exception
	*/
	public function __destruct()
	{
		$this->disconnect();
		$this->closelog();
	}
	/*
	* Connect to the pop3 server
	*
	* @param NULL|string $strHostname  Hostname or ip adress of a pop3 server
	* @param integer $intPort  The port for the pop3 service (default is 110)
	* @param array $arrConnectionTimeout  array("sec" => "", "usec" => "")
	* @param bool $bIPv6  IP Version 6 Protocol
	*
	* @throw POP3_Exception
	*/
	public function connect( &$strHostname , $intPort = 110, $arrConnectionTimeout = array("sec" => 10, "usec" => 0) ,$bIPv6 = FALSE )
	{
		$this->checkState(POP3::STATE_DISCONNECT);
		/// Parameter checks ///
		if( !is_string($strHostname) )
		{
			throw new POP3_Exception("Invalid host parameter given", self::ERR_PARAMETER);
		}

		if( !is_int($intPort) || $intPort < 1 || $intPort > 65535 )
		{
			throw new POP3_Exception("Invalid port parameter given", self::ERR_PARAMETER);
		}
		/* Deprecated: will do by the setSocketTimeout function
		if( !is_array($arrConnectionTimeout) || !is_int($arrConnectionTimeout["sec"]) || !is_int($arrConnectionTimeout["usec"]) )
		{
		throw new POP3_Exception("Invalid connection timeout parameter given", self::ERR_PARAMETER);
		}
		*/
		if( !is_bool($bIPv6) )
		{
			throw new POP3_Exception("Invalid IPv6 parameter given", self::ERR_PARAMETER);
		}

		$this->strHostname = $strHostname;
		$this->intPort = $intPort;

		/// Connecting ///
		if( $this->bUseSockets )
		{
			if( !$this->resSocket = @socket_create( (($bIPv6) ? AF_INET6 : AF_INET), SOCK_STREAM, SOL_TCP ) )
			{
				throw new POP3_Exception("", self::ERR_SOCKETS);
			}
			$this->log( ($bIPv6) ? "AF_INET6" : "AF_INET" ."-TCP Socket created (using sockets extension)");

			$this->setSockTimeout($arrConnectionTimeout);

			if( !@socket_connect($this->resSocket, $this->strHostname, $this->intPort)
			|| !@socket_getpeername($this->resSocket,$this->strIPAdress) )
			{
				throw new POP3_Exception("", self::ERR_SOCKETS);
			}
		}
		else
		{
			$dTimeout = (double) implode(".",$arrConnectionTimeout);
			if( !$this->resSocket = @fsockopen($this->strProtocol. "://" . $this->strHostname .":". $this->intPort, &$intErrno, &$strError, $dTimeout) )
			{
				throw new POP3_Exception( "[". $intErrno."] -- ". $strError, self::ERR_STREAM );
			}

			$this->setSockTimeout($arrConnectionTimeout);
			$this->strIPAdress = @gethostbyname($this->strHostname);
		}
		$this->bSocketConnected = TRUE;
		$this->log("Connected to ". $this->strProtocol . "://". $this->strIPAdress .":". $this->intPort ." [". $this->strHostname ."]");

		// Get the first response with, if APOP support avalible, the apop banner.
		$strBuffer = $this->recvString();
		$this->log($strBuffer);
		$this->parseBanner($strBuffer);
		$this->intCurState = self::STATE_AUTHORIZATION;
	}
	/*
	* Disconnect from the server.
	* CAUTION:
	* This function doesn't send the QUIT command to the server so all as delete marked emails won't delete.
	*
	* @return void
	* @throw POP3_Exception
	*/
	public function disconnect()
	{
		if( $this->bSocketConnected )
		{
			if( $this->bUseSockets )
			{
				if( @socket_close($this->resSocket) === FALSE )
				{
					throw new POP3_Exception("", self::ERR_SOCKETS);
				}
			}
			else
			{
				if( !@fclose($this->resSocket) )
				{
					throw new POP3_Exception("fclose(): Failed to close socket", self::ERR_STREAM);
				}
			}
			$this->bSocketConnected = FALSE;
			$this->log("Disconneted from ". $this->strIPAdress .":". $this->intPort ." [". $this->strHostname ."]" );
		}
	}
	/**
	 * Authorize to the pop3 server with your login datas.
	 *
	 * @param string $strUser  Username
	 * @param string $strPass  Password
	 * @param boolean $bApop  APOP Authorization Mechanism
	 *
	 * @return void
	 * @throw POP3_Exception
	 */
	public function login( $strUser, $strPass, $bAPOP = FALSE)
	{
		$this->checkState(self::STATE_AUTHORIZATION);
		if( !is_string($strUser) || strlen($strUser) == 0 )
		{
			throw new POP3_Exception("Invalid username string given", self::ERR_PARAMETER);
		}
		if( !is_string($strPass) )
		{
			throw new POP3_Exception("Invalid password string given", self::ERR_PARAMETER);
		}
		if( !is_bool($bAPOP) )
		{
			throw new POP3_Exception("Invalid APOP variable given", self::ERR_PARAMETER);
		}

		if( $this->bAPOPAutoDetect && !is_null($this->strAPOPBanner) && !$bAPOP)
		{
			$bAPOP = TRUE;
		}

		if( $bAPOP )
		{
			// APOP Auth
			$this->sendCmd("APOP ". $strUser ." ". hash("md5",$this->strAPOPBanner . $strPass, false), "APOP ". (($this->bHideUsernameAtLog) ? hash("sha256",$strUser . microtime(true),false) : $strUser) ." ". hash("md5",$this->strAPOPBanner . $strPass, false));
		}
		else
		{
			// POP3 Auth
			$this->sendCmd( "USER ". $strUser, "USER ". (($this->bHideUsernameAtLog) ? hash("sha256",$strUser . microtime(true),false) : $strUser) );
			$this->sendCmd( "PASS ". $strPass, "PASS ". hash("sha256",$strPass . microtime(true),false) );
		}
		$this->intCurState = self::STATE_TRANSACTION;
	}
	/**
	 * Send the quit command to the server.
     * All as delete marked messages will remove from the mail drop.
	 *
	 * @return void
	 * @throw POP3_Exception
	 */
	public function quit()
	{
		try
		{
			$this->checkState(self::STATE_TRANSACTION);
		}
		catch( POP3_Exception $e )
		{
			$this->checkState(self::STATE_AUTHORIZATION);
		}
		$this->sendCmd("QUIT");
	}
	/**
     * Get the stats from the pop3 server
     * This is only a string with the count of mails and their size in your mail drop.
     *
     * @return string  example: "+OK 2 3467"
     * @throw POP3_Exception
     */
	public function getStat()
	{
		$this->checkState(self::STATE_TRANSACTION);
		return $this->sendCmd("STAT");
	}
	/**
     * Recieve a raw message.
     *
     * @param int intMsgNum  The message number on the pop3 server.
     *
     * @return string  Complete message
     * @throw POP3_Exception
     */
	public function getMsg( $intMsgNum )
	{
		$this->checkState(self::STATE_TRANSACTION);
		$this->checkMsgNum($intMsgNum);
		$this->sendCmd("RETR ". $intMsgNum );
		return $this->recvToPoint();
	}
	/**
     * Get a list with message number and the size in bytes of a message.
     *
     * @return string  A String with a list of all message number and size in your mail drop seperated by "\r\n"
     * @throw POP3_Exception
     */
	public function getList()
	{
		$this->checkState(self::STATE_TRANSACTION);
		$this->sendCmd("LIST");
		return $this->recvToPoint();
	}
	/**
     * Get a list with message number and the unique id on the pop3 server. 
     *
     * @return string  Unique ID List
     * @throw POP3_Exception
     */
	public function getUidl()
	{
		$this->checkState(self::STATE_TRANSACTION);
		$this->sendCmd("UIDL");
		return $this->recvToPoint();
	}
	/**
     * Get the message header and if you want x lines of the message body.
     *
     * @param int intMsgNum  The message number on the pop3 server.
     * @param int intLines  The count of lines of the message body. (default is 0)
     *
     * @return string  Message header
     * @throw POP3_Exception
     */
	public function getTop( $intMsgNum , $intLines = 0 )
	{
		$this->checkState(self::STATE_TRANSACTION);
		$this->checkMsgNum($intMsgNum);
		if( !is_int($intLines) ) throw new POP3_Exception("Invalid line number given", self::ERR_PARAMETER);
		$this->sendCmd("TOP ". $intMsgNum ." ". $intLines);
		return $this->recvToPoint();
	}
	/**
     * Mark a message as delete
     *
     * @param int $intMsgNum  Message Number on the pop3 server
     *
     * @throw POP3_Exception
     */
	public function deleteMsg( $intMsgNum )
	{
		$this->checkState(self::STATE_TRANSACTION);
		$this->checkMsgNum($intMsgNum);
		$this->sendCmd("DELE ". $intMsgNum);
	}
	/**
     *
     * @param array $arrMsgNums  Numeric array with the message numbers on the pop3 server
     *
     * @return array  An array of messages stored under the message number
     * @throw POP3_Exception
     */
	public function getMails( $arrMsgNums )
	{
		$arrMsgs = array();
		foreach( $arrMsgNums as $intMsgNum )
		{
			$arrMsgs[$intMsgNum] = $this->getMsg($intMsgNum);
		}
		return $arrMsgs;
	}
	/**
     * Get the office status. That means that you will get an array
     * with all needed informations about your mail drop.
     * The array is build up like discribed here.
     *
     * $result = array( "count" => "Count of messages in your mail drop",
     *                  "octets" => "Size of your mail drop in bytes",
     *                  
     *                  "msg_number" => array("uid" => "The unique id string of the message on the pop3 server",
     *                                        "octets" => "The size of the message in bytes"
     *                                  ),
     *                  "and soon"
     *          );
     *
     * @return array  
     * @throw POP3_Exception
     */
	public function getOfficeStatus()
	{
		$this->checkState(self::STATE_TRANSACTION);
		$arrRes = array();

		$strSTATs = $this->getStat();
		$arrSTATs = explode(" ",trim($strSTATs));
		$arrRes["count"] = (int) $arrSTATs[1];
		$arrRes["octets"] = (int) $arrSTATs[2];

		if( $arrRes["count"] > 0 )
		{
			$strUIDLs = $this->getUidl();
			$strLISTs = $this->getList();

			$arrUIDLs = explode("\r\n",trim($strUIDLs));
			$arrLISTs = explode("\r\n",trim($strLISTs));

			for($i=1; $i<=$arrRes["count"]; $i++)
			{
				list(,$intUIDL) = explode(" ", trim($arrUIDLs[$i-1]));
				list(,$intLIST) = explode(" ", trim($arrLISTs[$i-1]));
				$arrRes[$i]["uid"] = $intUIDL;
				$arrRes[$i]["octets"] = (int) $intLIST;
			}
		}
		return $arrRes;
	}
	public function saveToFile( $strPathToFile, &$strMail )
	{
		//print_r($strMail);
		if( @is_file($strPathToFile) )
		{
			throw new POP3_Exception("File \"". $strPathToFile ."\" already exists", self::ERR_PARAMETER);
		}
		if( !$resFile = @fopen($strPathToFile,"w") )
		{
			throw new POP3_Exception("", self::ERR_STREAM);
		}
		if( !@fwrite($resFile,$strMail,strlen($strMail)) )
		{
			throw new POP3_Exception("", self::ERR_STREAM);
		}
		@fclose($resFile);

	}
	/*
	* This function store a message under their message number.
	* And that in the folder that was given by the path parameter.
	*
	* @param int $intMsgNum  Message Number on the server @see getOfficeStatus(), list()
	* @param string $strPathToDir  Path to the directory where the mail should be store.
	* @param string $strFileEnding  The file ending for a email (default ".eml")
	*
	* @return void
	* @throw POP3_Exception
	*/
	public function saveToFileFromServer( $intMsgNum, $strPathToDir = "./", $strFileEnding = ".eml" )
	{
		if( !@is_dir($strPathToDir) || !@is_writeable($strPathToDir) )
		{
			throw new POP3_Exception( $strPathToDir ." is not a directory or the directory is not writeable", self::ERR_PARAMETER);
		}
		$strPathToFile = $strPathToDir . $intMsgNum . $strFileEnding;
		$this->saveToFile($strPathToFile, $this->getMsg($intMsgNum));
	}
	/**
     *
     *
     */
	public function saveToSQL( &$strMail, &$resDBHandler, $strTable = "inbox" )
	{
		throw new POP3_Exception("",self::ERR_NOT_IMPLEMENTS);
	}
	public function saveToSQLFromServer( $intMsgNum, &$resDBHandler, $strTable = "inbox" )
	{
		throw new POP3_Exception("",self::ERR_NOT_IMPLEMENTS);
	}
	/**
     * Return the version of the pop3.class.inc
     * 
     * @return string $strVersion  version string for this class
     */
	public function insertToDB($mail)
	{
		$to=mysql_real_escape_string($mail['0']['To']);
		$string = $to;
		$matches = array();
		$pattern = '/[A-Za-z0-9_.+-]+@[A-Za-z0-9_-]+\.([A-Za-z0-9_-][A-Za-z0-9_]+)/';
		preg_match($pattern,$string,$matches);
		$to = $matches[0];
		$vardas=mysql_real_escape_string($mail['0']['vardas']);
		$adresas=mysql_real_escape_string($mail['0']['adresas']);
		$subject=mysql_real_escape_string($mail['0']['Subject']);
		$date=mysql_real_escape_string($mail['0']['Date']);
		// 	$disp.=  $timestamp = date( 'Y-m-d H:i:s O', strtotime( $date) );
		//	$disp.=  $date=date('Y-m-d H:i:s O',$timestamp);
		$papild=mysql_real_escape_string($mail['0']['Message']['papild']);
		$plain=mysql_real_escape_string($mail['0']['Message']['plain']);
		$html=mysql_real_escape_string($mail['0']['Message']['html']);

		$sql="insert into uzsakovai_inbox set
			adresatas = '{$to}',
			siunt_vardas = '{$vardas}', 
			siunt_adresas = '{$adresas}',
			tema = '{$subject}',
			data = date_format(str_to_date('{$date}','%a, %d %b %Y %H:%i:%S'),'%Y-%m-%d %H:%i:%S'),
			papild = '{$papild}',
			plain_text = '{$plain}',
			html_text = '{$html}'
			";
		//print $sql;
		$RS=mysql_query($sql);
	}
	public function getVersion()
	{
		return $this->strVersion;
	}

	/////////////////////////////////////////////////////////////////////////////
	/////////////////////// Private functions ///////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////

	/**
     * Compare the current state with the needed state.
     *
     * @param integer $intNeededState
     *
     * @throw POP3_Exception
     */
	private function checkState( $intNeededState )
	{
		if ( $this->intCurState != $intNeededState)
		throw new POP3_Exception("Invalid State !!! Please check your Code !!!", self::ERR_INVALID_STATE);
	}

	/**
	 * @param &integer $intMsgNum
	 * 
	 * @throws POP3_Exception
	 */
	private function checkMsgNum( &$intMsgNum )
	{
		if( !is_int($intMsgNum) )
		{
			throw new POP3_Exception("Invalid message number given", self::ERR_PARAMETER);
		}
	}
	/**
     * Send a string to the server.
     * Will append the network lineend "\r\n".
     *
     * @param string strCmd  The string that should send to the pop3 server
     * 
     * @return void
     * @throws POP3_Exception
     */
	private function send( $strCmd )
	{
		$strCmd .= "\r\n";
		if( $this->bUseSockets )
		{
			if( @socket_send($this->resSocket, $strCmd, strlen($strCmd), 0) === FALSE )
			{
				throw new POP3_Exception("", self::ERR_SOCKETS);
			}
		}
		else
		{
			if( !@fwrite($this->resSocket, $strCmd, strlen($strCmd)) )
			{
				throw new POP3_Exception("fwrite(): Failed to write string to socket",self::ERR_STREAM);
			}
		}
	}
	/**
     * This function send the command to the server and will get the response
     * If the command goes failed, the function will throw the POP3_Exception with the
     * ERR_SEND_CMD error code and the response as error message.
     *
     * @param string $strCmd  The string with the command for the pop3 server
     * @param string $strLog  Workaround for non clear passwords and usernames in log file
     *
     * @return string  Server response if it was successfull
     * @throws POP3_Exception
     */
	private function sendCmd( $strCmd , $strLog = NULL )
	{
	( !is_null($strLog) ) ? $this->log($strLog) : $this->log($strCmd);
	$this->send($strCmd);
	$strRes = $this->recvString();
	$this->log($strRes);
	// 1. the check for the strlen of the result is a workaround for some server who don't send something after the quit command
	// 2. should run with qmailer too...qmailer bug (pop3.class.inc) "." instead of "+OK" after RETR command
	if( strlen($strRes) > 0 && $strRes{0} == '-' )
	{
		throw new POP3_Exception(trim($strRes), self::ERR_SEND_CMD);
	}
	return $strRes;
	}
	/**
     * Return value:
     * -----------------------------

     a) on success returns number of bytes read

     b) in case of no data on line, returns zero and $buf will be set to NULL.

     c) on failure returns false, and $buf will be set to NULL.
     To get the error code/message, call the appropriate socket functions.

     d) in case of disconnect, the function returns either b) or c) which depends on how connection was closed from the other end.
     It returns 0 if the connection was closed gracefully with FIN squence and false if it was reset.
     *
     * @param &string $strBuffer
     * @param ineger $intBufferSize
     *
     * @return int number of recieved bytes
     * @throws POP3_Exception
     */
	private function recv( &$strBuffer, $intBufferSize = self::DEFAULT_BUFFER_SIZE )
	{
		$strBuffer = "";
		if( $this->bUseSockets )
		{
			$intReadBytes = @socket_recv($this->resSocket, $strBuffer, $intBufferSize, 0);
			if( $intReadBytes === FALSE )
			{
				throw new POP3_Exception("", POP3::ERR_SOCKETS);
			}
		}
		else
		{
			if( !$strBuffer = @fread($this->resSocket, $intBufferSize) )
			{
				throw new POP3_Exception("fread(): Couldn't recieve from socket", self::ERR_STREAM);
			}
		}
		return $intReadBytes;
	}
	/**
     * 
     * @param integer $intBufferSize
     * 
     * @return string $strBuffer Return the recieved String ended by "\r\n"
     * @throw POP3_Exception
     */
	private function recvString( $intBufferSize = self::DEFAULT_BUFFER_SIZE )
	{
		$strBuffer = "";
		if( $this->bUseSockets )
		{
			if( ($strBuffer = @socket_read($this->resSocket, $intBufferSize , PHP_NORMAL_READ)) === FALSE )
			{
				throw new POP3_Exception("", self::ERR_SOCKETS);
			}
			// Workaround: The socket_read function with PHP_NORMAL_READ stops at "\r" but the network string ends with "\r\n"
			// so we need to call the socket_read function again to get the "\n"
			if( ($strBuffer2 = @socket_read($this->resSocket, 1 , PHP_NORMAL_READ)) === FALSE )
			{
				throw new POP3_Exception("", self::ERR_SOCKETS);
			}
			$strBuffer .= $strBuffer2;
		}
		else
		{
			if( !$strBuffer = @fgets($this->resSocket, $intBufferSize) )
			{
				throw new POP3_Exception("fgets(): Couldn't recieve the string from socket", self::ERR_STREAM);
			}
		}
		return $strBuffer;
	}

	/**
     * This function will get a complete list/message until the finally point was sended.
     * 
     * @return string list/message
     * @throw POP3_Exception
     */
	private function recvToPoint()
	{
		$strRes = "";
		while(true)
		{
			$strBuffer = $this->recvString();
			$strRes .= $strBuffer;
			//print_r($strBuffer);
			if( strlen($strBuffer) == 3 && $strBuffer{0} == '.'  )
			{
				break;
			}
		}
		return $strRes;
	}

	/**
     * Set the connection timeouts for a socket
     *
     * @param array $arrTimeout  "sec" => seconds, "usec" => microseconds
     *
     * @return void
     * @throw POP3_Exception
     */
	private function setSockTimeout( $arrTimeout )
	{
		if( !is_array($arrTimeout) || !is_int($arrTimeout["sec"]) || !is_int($arrTimeout["usec"]) )
		{
			throw new POP3_Exception("Invalid Connection Timeout given", self::ERR_PARAMETER);
		}

		if( $this->bUseSockets )
		{
			if( !@socket_set_option($this->resSocket,SOL_SOCKET, SO_RCVTIMEO, $arrTimeout)
			|| !@socket_set_option($this->resSocket,SOL_SOCKET, SO_SNDTIMEO, $arrTimeout) )
			{
				throw new POP3_Exception("", self::ERR_SOCKETS);
			}
		}
		else
		{
			if( !@stream_set_timeout($this->resSocket, $arrTimeout["sec"], $arrTimeout["usec"]) )
			{
				throw new POP3_Exception("", self::ERR_STREAM);
			}
		}
		$this->log("Set socket timeout to ". implode(".",$arrTimeout) ." secondes.");
	}
	/**
     * Parse the needed apop banner if given
     * 
     * @return void
     */
	private function parseBanner( $strBuffer )
	{
		$intBufferLength = strlen($strBuffer);
		$bOpenTag = FALSE;
		for( $i=0; $i < $intBufferLength; $i++ )
		{
			if( $strBuffer{$i} == '>' )
			{
				break;
			}
			if( $bOpenTag )
			{
				$this->strAPOPBanner .= $strBuffer{$i};
				continue;
			}
			if( $strBuffer{$i} == '<' )
			{
				$bOpenTag = TRUE;
			}
		}
	}


	/**
     * // LOGGING FUNCTIONS
     */

	private function openlog()
	{
		if( !$this->bLogOpened && is_writeable($this->strLogFile) )
		{
			$disp.=  $this->strLogFile;
			if( !$this->resLogFp = fopen($this->strLogFile,"a+") )
			{
				throw new POP3_Exception("", self::ERR_LOG);
			}
			$this->bLogOpened = TRUE;
		}
		return $disp;
	}
	private function closelog()
	{
		if( $this->bLogOpened )
		{
			fclose($this->resLogFp);
			$this->bLogOpened = FALSE;
		}
	}
	private function log( $str )
	{
		if( $this->bLogOpened )
		{
			$str = date("Y-m-d H:i:s") .": ". trim($str) . PHP_EOL;
			if( !fwrite( $this->resLogFp, $str, strlen($str) ) )
			{
				return new POP3_Exception("", self::ERR_LOG);
			}
		}
	}
	// }}}


}
// }}}

?>
