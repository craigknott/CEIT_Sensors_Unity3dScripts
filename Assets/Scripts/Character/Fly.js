#if UNITY_WEBPLAYER

var flySpeed = 10;

var moveSpeed = 10;

var defaultCam : GameObject;
 
function Update()

{
    if (Input.GetKey(KeyCode.R))
    {

        transform.Translate(Vector3.up * flySpeed);

    }

    if (Input.GetKey(KeyCode.F))
    {

        transform.Translate(Vector3.down * flySpeed);

    }
    
    if (Input.GetKey(KeyCode.W))
    {
    
    	transform.Translate(Vector3.forward * moveSpeed);
    	
    }
    
    if (Input.GetKey(KeyCode.S))
    {
    
    	transform.Translate(Vector3.back * moveSpeed);
    	
    }
    
    if (Input.GetKey(KeyCode.A))
    {
    
    	transform.Translate(Vector3.left * moveSpeed);
    	
    }

	if (Input.GetKey(KeyCode.D))
    {
    
    	transform.Translate(Vector3.right * moveSpeed);
    	
    }
}

#endif