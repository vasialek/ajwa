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
    private $_mailUsername = "username@gmail.com";
    private $_mailPassword = "password";
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

    /**
     * Get email from server by POP3 protocol
     *
     * @return array
     * @throw POP3_Exception
     */

    function getEmails()
    {
        $objMails = new mail_vaizdas();

        /**
         * Remember that the encryption support doesn't work at time for the socket extension
         * This will I implement later.
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
            $mailArr = array();
            $i = 0;

            for($intMsgNum = 1; $intMsgNum <= $arrOfficeStatus["count"]; $intMsgNum++ )
            {
                $EML1=$objPOP3->getMsg($intMsgNum);

                $mail_obj = new MIMEDECODE($EML1, "\r\n");
                $mail = $mail_obj->get_parsed_message();

                $mailArr[$i] = $mail;
//                var_dump($mailArr);
                $i++;
            }

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

            return $mailArr;

        } catch (POP3_Exception $e) {
            die($e);
        }

        return $mailArr;
    }
}

// Your next code

$mail = new AiMail();
$mailData = $mail->getEmails();
//var_dump($mailData);

for ($i = 0; $i < count($mailData); $i++)
{
    echo "Date: " . date("Y-m-d H:i:s", strtotime($mailData[$i][0]['Date'])) . "<br />";
    echo "From: " . $mailData[$i][0]['adresas'] . "<br />";
    echo "First name: " . $mailData[$i][0]['vardas'] . "<br />";
    echo 'To: ' . $mailData[$i][0]['To'] . "<br />";
    echo "Subject: " . $mailData[$i][0]['Subject'] . "<br />";
    echo "Message (plain): " . $mailData[$i][0]['Message']['plain'] . "<br />";
    echo "Message (html): " . $mailData[$i][0]['Message']['html'] . "<br /><br />";
    echo "----------------------------------------<br />";
}

?> 
