# Simple Sample #

```

public ActionResult GetMyProfile(){

   MyProfile profile = new MyProfile(userName);

   return Xml(profile);
}

```