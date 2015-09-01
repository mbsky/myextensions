# Introduction #

GetEntitySetName Sample Code.


# Details #

MyEntities ctx = new MyEntities();

string contactEntitySetName = ctx.GetEntitySetName(tyof(Contact));