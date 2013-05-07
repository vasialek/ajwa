<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);
$strRootPath = 'mail';

require_once($strRootPath . "/class/mail.class.php");
require_once($strRootPath . "/class/mail_decode.class.php");

class AiMail
{
    private $_mailHost = "smtp.gmail.com";
    private $_mailProtocol = "tls";
    private $_mailUsername = "test@prado.lt";
    private $_mailPassword = "D9&4VPgy";
    private $_mailPort = 995;

    public function getMailHost()
    {
        return $this->_mailHost;
    }

    public function getMailProtocol()
    {
        return $this->_mailProtocol;
    }

    public function getMailUsername()
    {
        return $this->_mailUsername;
    }

    public function getMailPassword()
    {
        return $this->_mailPassword;
    }

    public function getMailPort()
    {
        return $this->_mailPort;
    }

    function getEmails($darid = '0', $adrid = '0', $uzid = '0', $box = '0', $mail = '0', $rodyti = '0')
    {
        $objMails = new mail_vaizdas();

        /**
         * Remember that the encryption support doesn't work at time for the socket extension
         * This will I implement later.
         *
         */
        $bUseSockets = FALSE;
        $bUseTLS = TRUE;
        $bIPv6 = FALSE;
        $arrConnectionTimeout = array("sec" => 10,
            "usec" => 500);

        // POP3 Options
        $bAPopAutoDetect = TRUE;
        $bHideUsernameAtLog = FALSE;

        // Logging Options
        $strLogFile = "php://stdout"; //$strRootPath. "pop3.log";

        // EMail store Sptions
        $strPathToDir = $strRootPath . "mails" . DIRECTORY_SEPARATOR;
        $strFileEndings = ".eml";


        try {
            // Instance the POP3 object
            $objPOP3 = new POP3($strLogFile, $bAPopAutoDetect, $bHideUsernameAtLog, $this->getMailProtocol(), $bUseSockets);

            // Connect to the POP3 server
            $objPOP3->connect($this->getMailHost(), $this->getMailPort(), $arrConnectionTimeout, $bIPv6);

            // Logging in
            $objPOP3->login($this->getMailUsername(), $this->getMailPassword());

            // Get the office status
            $arrOfficeStatus = $objPOP3->getOfficeStatus();

            /**
             * This for loop store the messages under their message number on the server
             * and mark the message as delete on the server.
             */
            //"Hello world!";
            for ($intMsgNum = 1; $intMsgNum <= $arrOfficeStatus["count"]; $intMsgNum++) {
                $EML1 = $objPOP3->getMsg($intMsgNum);

                $mail_obj = new MIMEDECODE($EML1, "\r\n");
                $mail = $mail_obj->get_parsed_message();
                $i++;


                //$objPOP3->insertToDB($mail);
                //$objPOP3->saveToFileFromServer($intMsgNum, $strPathToDir, $strFileEndings);
                //        $objPOP3->deleteMsg($intMsgNum);

            }

            //$disp = "<div class='td-thead-l'>Gauta naujų laiškų: <b>{$arrOfficeStatus['count']}</b></div></br>";

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

            /*if ($mail=='new')
            {
                $disp.= $objMails->displayNewMail();
                if(!empty($_POST['send'])) $disp.=$objMails->sendMail($this->getMailUsername(), $this->_mailPassword, $rez->d_email);
                if(!empty($_POST['save'])) $disp.=$objMails->saveMail($this->getMailUsername(), $this->getMailPassword(), $rez->d_email);
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
                if($box=='siustieji') $arr = $objMails->getMails(null, $this->getMailUsername());
                elseif($box=='juodrastis') $arr = $objMails->getMails(null,null,'1');
                else $arr = $objMails->getMails($this->getMailUsername(),null);
                $disp.= $objMails->displayWebMail($arr);
            }*/

            //var_dump('testas: ' . $objMails->getMails('test@prado.lt', null));

            return $mail;
        } catch (POP3_Exception $e) {
            die($e);
        }
        return $disp;
    }
}

// Your next code

$mail = new AiMail();
$mailData = $mail->getEmails();
//var_dump($mailData);
for ($i = 0; $i < count($mailData); $i++) {
    echo "Date: " . $mailData[$i]['Date'] . "<br />";
    echo "From: " . $mailData[$i]['adresas'] . "<br />";
    echo "First name: " . $mailData[$i]['vardas'] . "<br />";
    echo 'To: ' . $mailData[$i]['To'] . "<br />";
    echo "Subject: " . $mailData[$i]['Subject'] . "<br />";
    echo "Message (plain): " . $mailData[$i]['Message']['plain'] . "<br />";
    echo "Message (html): " . $mailData[$i]['Message']['html'] . "<br />";
}

?> 
