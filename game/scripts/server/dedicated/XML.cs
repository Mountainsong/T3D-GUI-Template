
//-----------------------------------------------------------------------------
// Login Client - Create new if client doesn't exist.
//-----------------------------------------------------------------------------
/* Here we pass the Character Name, Login, and PW data 
	the player entered in previous GUI windows.
	If the Login and PW in clientData.xml check out on the current server, 
	the stored Character Data is loaded from characterData.xml.
	
	Character Data is stored on the server only.
*/

function loginClient( %name, %login, %pw )
{
	%xml = new SimXMLDocument();
	%xml.loadFile("./scripts/server/dedicated/clientData.xml");

	%xml.pushChildElement("Clients");
	while (true)
	{
		// Check if we have a client with this login.
		%search = %xml.pushFirstChildElement( %login );
		while (%search == true)
		{
			%searchPW = %xml.attribute("PW");
			
			if ( %searchPW == %pw )
			{
				echo ("Login Success!");

				// Load up saved character.
				//%result = loadCharacter (%name, %login, %pw );			
			}
			else 
			{
				echo ("Incorrect Password.");
				return;
			}
		}			
		
		if (%search == false)
		{
			// If we don't, create the NewElement.
			%xml.pushNewElement( %login );
			
			//change data
			%xml.setAttribute( "Login", %login );
			%xml.setAttribute( "PW", %pw );
			
			%xml.saveFile("./scripts/server/dedicated/clientData.xml");
		}
		
		%xml.popElement();
		break;
	}
}

//-----------------------------------------------------------------------------
// Save Character Data
//-----------------------------------------------------------------------------

function saveCharacter( %name, %login, %pw, %skin )
{
	%xml = new SimXMLDocument();
	%xml.loadFile("./scripts/server/dedicated/characterData.xml");
	
	%xml.pushChildElement("Characters");
	
	// Check if we already have a character with this name.
	%search = %xml.pushFirstChildElement( %name );
	
	if (%search == false)
	{
		// If we don't, create the NewElement.
		%xml.pushNewElement( %name );
		
		//store data
		%xml.setAttribute( "Login", %login );
		%xml.setAttribute( "PW", %pw );
		%xml.setAttribute( "Skin", %skin );
		
		%xml.saveFile("./scripts/server/dedicated/characterData.xml");
	}
	else
	{
		// If we do, open the existing ChildElement.
		%xml.pushChildElement( %name );
		while (true)
		{
			%searchLogin = %xml.attribute ("Login");
			%searchPW = %xml.attribute ("PW");
			if ( %searchLogin == %login && %searchPW == %pw)
			{
				echo ("Existing character data being overwritten...");
			
				//change data
				%xml.setAttribute( "Skin", %skin );
				
				%xml.saveFile("./scripts/server/dedicated/characterData.xml");
				return;
			}
			
			else
			{
				return;
			}
		}
		
		%xml.popElement();	
	}
}

//-----------------------------------------------------------------------------
// Load Character Data
//-----------------------------------------------------------------------------

function loadCharacter( %name, %login, %pw, %skin )
{
	%xml = new SimXMLDocument();
	%xml.loadFile("./scripts/server/dedicated/characterData.xml");

	%xml.pushChildElement("Characters");

	// Check if we already have a character with this name.
	%search = %xml.pushFirstChildElement( %name );
		
    if (%search == false)
    {
        echo ("No character with that name.");
        %result = chooseSkin(%skin);
        return %result;
    }

	else 
	{
        %xml.pushChildElement(%name);
		%searchLogin = %xml.attribute("Login");
			
		if (%searchLogin == %login)
		{
			%searchPW = %xml.attribute("PW");
				
			if (%searchPW == %pw)
			{
				echo ("Character Load Successful.");
				%result = %xml.attribute("Skin");
			}
				
			else
			{
				echo("Incorrect Password.");
				return;
			}
		}

		else
		{
			echo ("Incorrect Login.");
			return;
		}
	}	
		
	%xml.popElement();
    return %result;
}

//-----------------------------------------------------------------------------
// Character Skins
//-----------------------------------------------------------------------------

function chooseSkin( %skin )
{
	%xml = new SimXMLDocument();
	%xml.loadFile("./scripts/server/dedicated/armorSkins.xml");

	%xml.pushChildElement("Skins");

	// Check if we have a skin with this name.
	%search = %xml.pushFirstChildElement( %skin );
		
    if (%search == false)
    {
        echo ("No skins with that name.");
        return;
    }

	else 
	{
        %xml.pushChildElement(%skin);
		
        echo ("Skin selection successful.");
		%result = %xml.attribute("Skin");
	}
	
	%xml.popElement();
    return %result;
}