
SELECT 
	 T_Ref_Region.RG_UID
	,T_Ref_Region.RG_Lang_de
	,T_Ref_Region.RG_Lang_fr
	,T_Ref_Region.RG_Lang_it
	,T_Ref_Region.RG_Lang_en
	
	,T_Ref_Location.LC_UID
	,T_Ref_Location.LC_DwgKurz

	,T_Ref_Location.LC_ID_SR

	,T_Ref_Location.LC_Kurz_de
	,T_Ref_Location.LC_Kurz_fr
	,T_Ref_Location.LC_Kurz_it
	,T_Ref_Location.LC_Kurz_en

	,T_Ref_Location.LC_Lang_de
	,T_Ref_Location.LC_Lang_fr
	,T_Ref_Location.LC_Lang_it
	,T_Ref_Location.LC_Lang_en
	
	,T_Ref_Country.CTR_UID
	,T_Ref_Country.CTR_Code
	,T_Ref_Country.CTR_AreaCode
	
	,T_Ref_Country.CTR_ID_SR

	,T_Ref_Country.CTR_Kurz_de
	,T_Ref_Country.CTR_Kurz_fr
	,T_Ref_Country.CTR_Kurz_it
	,T_Ref_Country.CTR_Kurz_en

	,T_Ref_Country.CTR_Lang_de
	,T_Ref_Country.CTR_Lang_fr
	,T_Ref_Country.CTR_Lang_it
	,T_Ref_Country.CTR_Lang_en
	
	,T_Ref_Country.CTR_DWG

	
	,T_Premises.PR_UID
	,T_Premises.PR_Name

	,T_Premises.PR_BuildingAdr_ZIP

	,T_Premises.PR_DwgKurz
	,T_Premises.PR_ID_SR

	
	,T_Floor.FL_UID 
	,T_Floor.FL_Level
	,T_Floor.FL_Floor
	,T_Floor.FL_Sort

	,T_Floor.FL_ID_SR
	
	,T_Ref_FloorType.FT_UID
	,T_Ref_FloorType.FT_Sort
	
	,T_Ref_FloorType.FT_Kurz_de
	,T_Ref_FloorType.FT_Kurz_fr
	,T_Ref_FloorType.FT_Kurz_it
	,T_Ref_FloorType.FT_Kurz_en	
	
	
	,T_Ref_FloorType.FT_Lang_de
	,T_Ref_FloorType.FT_Lang_fr
	,T_Ref_FloorType.FT_Lang_it
	,T_Ref_FloorType.FT_Lang_en
	

	,T_Room.RM_UID
	,T_Room.RM_RoomNumber

	,T_Room.RM_RoomName
	,T_Room.RM_ID_SR

	,T_ZO_Premises_DWG.ZO_PRDWG_ApertureDWG
	,T_ZO_Premises_DWG.ZO_PRDWG_ApertureObjID
	
	,T_ZO_Floor_DWG.ZO_FLDWG_ApertureDWG
	,T_ZO_Floor_DWG.ZO_FLDWG_ApertureObjID
	,T_ZO_Floor_Area.ZO_FLArea_Area
	
	,T_ZO_Room_DWG.ZO_RMDWG_ApertureDWG
	,T_ZO_Room_DWG.ZO_RMDWG_ApertureObjID
FROM T_Ref_Location 

LEFT JOIN T_Ref_Country
	ON T_Ref_Country.CTR_UID = T_Ref_Location.LC_CTR_UID 
	AND T_Ref_Country.CTR_Status = 1 
	
LEFT JOIN T_Ref_Region
	ON T_Ref_Region.RG_UID = T_Ref_Country.CTR_RG_UID 
	AND T_Ref_Region.RG_Status = 1 
	
LEFT JOIN T_Premises 
	ON T_Premises.PR_LC_UID = LC_UID 
	AND T_Premises.PR_Status = 1 
	AND {fn curdate()} BETWEEN T_Premises.PR_DateFrom AND T_Premises.PR_DateTo 
	
LEFT JOIN T_ZO_Premises_DWG
	ON T_ZO_Premises_DWG.ZO_PRDWG_PR_UID = PR_UID 
	AND T_ZO_Premises_DWG.ZO_PRDWG_Status = 1 
	AND {fn curdate()} BETWEEN  T_ZO_Premises_DWG.ZO_PRDWG_DateFrom AND T_ZO_Premises_DWG.ZO_PRDWG_DateTo 
	
LEFT JOIN T_Floor 
	ON T_Floor.FL_PR_UID = PR_UID 
	AND T_Floor.FL_Status = 1 
	AND {fn curdate()} BETWEEN  T_Floor.FL_DateFrom AND T_Floor.FL_DateTo 
	
LEFT JOIN T_Ref_FloorType
	ON T_Ref_FloorType.FT_UID = T_Floor.FL_FT_UID 
	AND T_Ref_FloorType.FT_Status = 1 
	
LEFT JOIN T_ZO_Floor_DWG 
	ON T_ZO_Floor_DWG.ZO_FLDWG_FL_UID = T_Floor.FL_UID 
	AND T_ZO_Floor_DWG.ZO_FLDWG_Status = 1 
	AND {fn curdate()} BETWEEN  T_ZO_Floor_DWG.ZO_FLDWG_DateFrom AND T_ZO_Floor_DWG.ZO_FLDWG_DateTo 
	
LEFT JOIN T_ZO_Floor_Area
	ON T_ZO_Floor_Area.ZO_FLArea_FL_UID = T_Floor.FL_UID 
	AND T_ZO_Floor_Area.ZO_FLArea_Status = 1 
	AND {fn curdate()} BETWEEN  T_ZO_Floor_Area.ZO_FLArea_DateFrom AND T_ZO_Floor_Area.ZO_FLArea_DateTo 
	
LEFT JOIN T_Room 
	ON T_Room.RM_FL_UID = FL_UID 
	AND T_Room.RM_Status = 1 
	AND {fn curdate()} BETWEEN  T_Room.RM_DateFrom AND T_Room.RM_DateTo 
	
LEFT JOIN T_ZO_Room_DWG
	ON T_ZO_Room_DWG.ZO_RMDWG_RM_UID = RM_UID 
	AND T_ZO_Room_DWG.ZO_RMDWG_Status = 1 
	AND {fn curdate()} BETWEEN  T_ZO_Room_DWG.ZO_RMDWG_DateFrom AND T_ZO_Room_DWG.ZO_RMDWG_DateTo 
	
WHERE (1=1) 
AND T_Ref_Location.LC_Status = 1
