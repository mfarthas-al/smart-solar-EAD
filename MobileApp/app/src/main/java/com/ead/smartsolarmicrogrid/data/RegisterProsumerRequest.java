package com.ead.smartsolarmicrogrid.data;

// Matches WebService's RegisterProsumerRequest DTO.
public class RegisterProsumerRequest {
    public String nic;
    public String fullName;
    public String email;
    public String password;
    public String contactNumber;
    public String propertyAddress;
    public Double solarPanelCapacityKw;

    public RegisterProsumerRequest(String nic, String fullName, String email, String password,
                                    String contactNumber, String propertyAddress, Double solarPanelCapacityKw) {
        this.nic = nic;
        this.fullName = fullName;
        this.email = email;
        this.password = password;
        this.contactNumber = contactNumber;
        this.propertyAddress = propertyAddress;
        this.solarPanelCapacityKw = solarPanelCapacityKw;
    }
}
